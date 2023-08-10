using Confluent.Kafka;
using Infrastructure.Converters;
using Infrastructure.Handlers;
using Microsoft.Extensions.Options;
using SharedKernel.Consumers;
using SharedKernel.Events;
using System;
using System.Reflection;
using System.Text.Json;

namespace Infrastructure.Consumers
{
    public class EventConsumer : IEventConsumer
    {
        private readonly ConsumerConfig _config;
        private readonly IEventHandler _eventHandler;

        public EventConsumer(IOptions<ConsumerConfig> config, IEventHandler eventHandler)
        {
            _config = config.Value;
            _eventHandler = eventHandler;
        }

        public void Consume(string topic)
        {
            using (IConsumer<string, string> consumer = new ConsumerBuilder<string, string>(_config)
                .SetKeyDeserializer(Deserializers.Utf8)
                .SetValueDeserializer(Deserializers.Utf8)
                .Build())
            {
                consumer.Subscribe(topic);

                while (true)
                {
                    ConsumeResult<string, string> consumeResult = consumer.Consume();

                    if (consumeResult?.Message is null)
                    {
                        continue;
                    }

                    JsonSerializerOptions options = new()
                    {
                        Converters = { new EventJsonConverter() }
                    };

                    BaseEvent @event = JsonSerializer.Deserialize<BaseEvent>(consumeResult.Message.Value, options);

                    MethodInfo handlerMethod = _eventHandler.GetType().GetMethod("On", new Type[] { @event.GetType() });

                    if (handlerMethod is null)
                    {
                        throw new ArgumentNullException(nameof(handlerMethod), "Could not find event handler method!");
                    }

                    handlerMethod.Invoke(_eventHandler, new object[] { @event });
                    consumer.Commit(consumeResult);
                }
            }
        }
    }
}