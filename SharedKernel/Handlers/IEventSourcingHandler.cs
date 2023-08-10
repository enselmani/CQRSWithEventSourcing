using SharedKernel.Domain;
using System;
using System.Threading.Tasks;

namespace SharedKernel.Handlers
{
    public interface IEventSourcingHandler<T>
    {
        Task SaveAsync(AggregateRoot aggregate);

        Task<T> GetByIdAsync(Guid aggregateId);

        Task RepublishEventsAsync();
    }
}