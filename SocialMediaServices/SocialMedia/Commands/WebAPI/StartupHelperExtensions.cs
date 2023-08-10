using Common.Events;
using Confluent.Kafka;
using Domain.Aggregates;
using Infrastructure.Config;
using Infrastructure.Dispatchers;
using Infrastructure.Handlers;
using Infrastructure.Producers;
using Infrastructure.Repositories;
using Infrastructure.Stores;
using Microsoft.AspNetCore.Builder;
using MongoDB.Bson.Serialization;
using SharedKernel.Domain;
using SharedKernel.Events;
using SharedKernel.Handlers;
using SharedKernel.Infrastructure;
using SharedKernel.Producers;
using WebAPI.Commands.CommandHandler;
using WebAPI.Commands;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.DependencyInjection;

namespace WebAPI
{
    public static class StartupHelperExtensions
    {
        // Add Services to the container
        public static WebApplication ConfigureServices(this WebApplicationBuilder builder)
        {
            BsonClassMap.RegisterClassMap<BaseEvent>();
            BsonClassMap.RegisterClassMap<PostCreatedEvent>();
            BsonClassMap.RegisterClassMap<MessageUpdatedEvent>();
            BsonClassMap.RegisterClassMap<PostLikedEvent>();
            BsonClassMap.RegisterClassMap<CommentAddedEvent>();
            BsonClassMap.RegisterClassMap<CommentUpdatedEvent>();
            BsonClassMap.RegisterClassMap<CommentRemovedEvent>();
            BsonClassMap.RegisterClassMap<PostRemovedEvent>();

            builder.Services.Configure<MongoDbConfig>(builder.Configuration.GetSection(nameof(MongoDbConfig)));
            builder.Services.Configure<ProducerConfig>(builder.Configuration.GetSection(nameof(ProducerConfig)));

            builder.Services.AddScoped<IEventStoreRepository, EventStoreRepository>();
            builder.Services.AddScoped<IEventProducer, EventProducer>();
            builder.Services.AddScoped<IEventStore, EventStore>();
            builder.Services.AddScoped<IEventSourcingHandler<PostAggregate>, EventSourcingHandler>();
            builder.Services.AddScoped<ICommandHandler, CommandHandler>();

            // register command handler methods
            ICommandHandler commandHandler = builder.Services.BuildServiceProvider().GetRequiredService<ICommandHandler>();
            CommandDispatcher dispatcher = new();

            dispatcher.RegisterHandler<NewPostCommand>(commandHandler.HandleAsync);
            dispatcher.RegisterHandler<EditMessageCommand>(commandHandler.HandleAsync);
            dispatcher.RegisterHandler<LikePostCommand>(commandHandler.HandleAsync);
            dispatcher.RegisterHandler<AddCommentCommand>(commandHandler.HandleAsync);
            dispatcher.RegisterHandler<EditCommentCommand>(commandHandler.HandleAsync);
            dispatcher.RegisterHandler<RemoveCommentCommand>(commandHandler.HandleAsync);
            dispatcher.RegisterHandler<DeletePostCommand>(commandHandler.HandleAsync);
            dispatcher.RegisterHandler<RestoreReadDatabaseCommand>(commandHandler.HandleAsync);
            builder.Services.AddSingleton<ICommandDispatcher>(_ => dispatcher);

            builder.Services.AddControllers();

            builder.Services.AddEndpointsApiExplorer();

            builder.Services.AddSwaggerGen();

            return builder.Build();
        }

        // Configure the HTTP request/response pipeline
        public static WebApplication ConfigurePipeline(this WebApplication app)
        {
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();

            app.MapControllers();

            return app;
        }
    }
}