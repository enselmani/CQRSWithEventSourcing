using Infrastructure.Config;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using SharedKernel.Domain;
using SharedKernel.Events;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Infrastructure.Repositories
{
    public class EventStoreRepository : IEventStoreRepository
    {
        private readonly IMongoCollection<EventModel> _eventStoreCollection;

        public EventStoreRepository(IOptions<MongoDbConfig> config)
        {
            MongoClient mongoClient = new();
            IMongoDatabase mongoDatabase = mongoClient.GetDatabase(config.Value.Database);

            _eventStoreCollection = mongoDatabase.GetCollection<EventModel>(config.Value.Collection);
        }

        public async Task<List<EventModel>> FindByAggregateId(Guid aggregateId)
        {
            return await _eventStoreCollection
                .Find(x => x.AggregateIdentifier == aggregateId)
                .ToListAsync()
                .ConfigureAwait(false);
        }

        public async Task<List<EventModel>> FindAllAsync()
        {
            return await _eventStoreCollection
                .Find(_ => true)
                .ToListAsync()
                .ConfigureAwait(false);
        }

        public async Task SaveAsync(EventModel @event)
        {
            await _eventStoreCollection.InsertOneAsync(@event).ConfigureAwait(false);
        }
    }
}