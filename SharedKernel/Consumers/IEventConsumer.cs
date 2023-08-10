namespace SharedKernel.Consumers
{
    public interface IEventConsumer
    {
        void Consume(string topic);
    }
}