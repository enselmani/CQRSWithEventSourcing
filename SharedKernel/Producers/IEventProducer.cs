using SharedKernel.Events;
using System.Threading.Tasks;

namespace SharedKernel.Producers
{
    public interface IEventProducer
    {
        Task ProduceAsync<T>(string topic, T @event) where T : BaseEvent;
    }
}