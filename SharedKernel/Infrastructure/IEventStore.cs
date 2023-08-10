using SharedKernel.Events;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SharedKernel.Infrastructure
{
    public interface IEventStore
    {
        Task SaveEventsAsync(Guid aggregateId, IEnumerable<BaseEvent> events, int expectedVersion);

        Task<List<BaseEvent>> GetEventsAsync(Guid aggregateId);

        Task<List<Guid>> GetAggregateIdsAsync();
    }
}