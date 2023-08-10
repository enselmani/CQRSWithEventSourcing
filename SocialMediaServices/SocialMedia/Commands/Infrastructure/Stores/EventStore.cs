using Domain.Aggregates;
using SharedKernel.Domain;
using SharedKernel.Events;
using SharedKernel.Exceptions;
using SharedKernel.Infrastructure;
using SharedKernel.Producers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Infrastructure.Stores
{
    public class EventStore : IEventStore
    {
        private readonly IEventStoreRepository _eventStoreRepository;
        private readonly IEventProducer _eventProducer;

        public EventStore(IEventStoreRepository eventStoreRepository, IEventProducer eventProducer)
        {
            _eventStoreRepository = eventStoreRepository;
            _eventProducer = eventProducer;
        }

        public async Task<List<BaseEvent>> GetEventsAsync(Guid aggregateId)
        {
            List<EventModel> eventStream = await _eventStoreRepository.FindByAggregateId(aggregateId);

            if (eventStream is null || !eventStream.Any())
            {
                throw new AggregateNotFoundException("Incorrect post ID provided!");
            }

            return eventStream.OrderBy(x => x.Version).Select(x => x.EventData).ToList();
        }

        public async Task SaveEventsAsync(Guid aggregateId, IEnumerable<BaseEvent> events, int expectedVersion)
        {
            List<EventModel> eventStream = await _eventStoreRepository.FindByAggregateId(aggregateId);

            if (expectedVersion != -1 && eventStream[^1].Version != expectedVersion)
            {
                throw new ConcurrencyException();
            }

            int version = expectedVersion;

            foreach (BaseEvent @event in events)
            {
                version++;
                @event.Version = version;
                string eventType = @event.GetType().Name;
                EventModel eventModel = new()
                {
                    TimeStamp = DateTime.Now,
                    AggregateIdentifier = aggregateId,
                    AggregateType = nameof(PostAggregate),
                    Version = version,
                    EventType = eventType,
                    EventData = @event
                };

                await _eventStoreRepository.SaveAsync(eventModel);

                string topic = Environment.GetEnvironmentVariable("KAFKA_TOPIC");
                await _eventProducer.ProduceAsync(topic, @event);
            }
        }

        public async Task<List<Guid>> GetAggregateIdsAsync()
        {
            List<EventModel> eventStream = await _eventStoreRepository.FindAllAsync();

            if (eventStream is null || !eventStream.Any())
            {
                throw new ArgumentNullException(nameof(eventStream), "Could not retrieve event stream from the event store!");
            }

            return eventStream.Select(x => x.AggregateIdentifier)
                .Distinct()
                .ToList();
        }
    }
}