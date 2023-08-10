using SharedKernel.Events;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SharedKernel.Domain
{
    public interface IEventStoreRepository
    {
        Task SaveAsync(EventModel @event);

        Task<List<EventModel>> FindByAggregateId(Guid aggregateId);

        Task<List<EventModel>> FindAllAsync();
    }
}