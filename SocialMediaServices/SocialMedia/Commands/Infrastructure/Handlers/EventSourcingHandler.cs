using Domain.Aggregates;
using SharedKernel.Domain;
using SharedKernel.Events;
using SharedKernel.Handlers;
using SharedKernel.Infrastructure;
using SharedKernel.Producers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Infrastructure.Handlers
{
    public class EventSourcingHandler : IEventSourcingHandler<PostAggregate>
    {
        private readonly IEventStore _eventStore;
        private readonly IEventProducer _eventProducer;

        public EventSourcingHandler(IEventStore eventStore, IEventProducer eventProducer)
        {
            _eventStore = eventStore;
            _eventProducer = eventProducer;
        }

        public async Task<PostAggregate> GetByIdAsync(Guid aggregateId)
        {
            PostAggregate aggregate = new();
            List<BaseEvent> events = await _eventStore.GetEventsAsync(aggregateId);

            if (events is null || !events.Any())
            {
                return aggregate;
            }

            aggregate.ReplayEvents(events);
            aggregate.Version = events.Select(x => x.Version).Max();

            return aggregate;
        }

        public async Task RepublishEventsAsync()
        {
            List<Guid> aggregateIds = await _eventStore.GetAggregateIdsAsync();

            if (aggregateIds is null || !aggregateIds.Any())
            {
                return;
            }

            foreach (Guid id in aggregateIds)
            {
                PostAggregate aggregate = await GetByIdAsync(id);

                if (aggregate is null || !aggregate.Active)
                {
                    continue;
                }

                List<BaseEvent> events = await _eventStore.GetEventsAsync(id);

                foreach (BaseEvent @event in events)
                {
                    string topic = Environment.GetEnvironmentVariable("KAFKA_TOPIC");
                    await _eventProducer.ProduceAsync(topic, @event);
                }
            }
        }

        public async Task SaveAsync(AggregateRoot aggregate)
        {
            await _eventStore.SaveEventsAsync(aggregate.Id, aggregate.GetUncommittedChanges(), aggregate.Version);
            aggregate.MarkChangesAsCommitted();
        }
    }
}