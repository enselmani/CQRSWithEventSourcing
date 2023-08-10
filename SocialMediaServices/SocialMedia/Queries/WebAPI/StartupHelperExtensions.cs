using Confluent.Kafka;
using Domain.Entities;
using Domain.Repositories;
using Infrastructure.Consumers;
using Infrastructure.Dispatchers;
using Infrastructure.Handlers;
using Infrastructure.Persistence;
using Infrastructure.Repositories;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using SharedKernel.Consumers;
using SharedKernel.Infrastructure;
using System;
using WebAPI.Queries;
using WebAPI.Queries.QueryHandler;
using EventHandler = Infrastructure.Handlers.EventHandler;

namespace WebAPI
{
    public static class StartupHelperExtensions
    {
        // Add Services to the container
        public static WebApplication ConfigureServices(this WebApplicationBuilder builder)
        {
            Action<DbContextOptionsBuilder> dbContextConfiguration = (dbContext) =>
            {
                dbContext.UseSqlServer(builder.Configuration.GetConnectionString("SqlServer"));
            };

            builder.Services.AddDbContext<DatabaseContext>(dbContextConfiguration);
            builder.Services.AddSingleton<DatabaseContextFactory>(new DatabaseContextFactory(dbContextConfiguration));

            builder.Services.AddScoped<IPostRepository, PostRepository>();
            builder.Services.AddScoped<ICommentRepository, CommentRepository>();
            builder.Services.AddScoped<IQueryHandler, QueryHandler>();
            builder.Services.AddScoped<IEventHandler, EventHandler>();
            builder.Services.Configure<ConsumerConfig>(builder.Configuration.GetSection(nameof(ConsumerConfig)));
            builder.Services.AddScoped<IEventConsumer, EventConsumer>();

            // register query handler methods
            IQueryHandler queryHandler = builder.Services.BuildServiceProvider().GetRequiredService<IQueryHandler>();
            QueryDispatcher dispatcher = new();

            dispatcher.RegisterHandler<FindAllPostsQuery>(queryHandler.HandleAsync);
            dispatcher.RegisterHandler<FindPostByIdQuery>(queryHandler.HandleAsync);
            dispatcher.RegisterHandler<FindPostsByAuthorQuery>(queryHandler.HandleAsync);
            dispatcher.RegisterHandler<FindPostsWithCommentsQuery>(queryHandler.HandleAsync);
            dispatcher.RegisterHandler<FindPostsWithLikesQuery>(queryHandler.HandleAsync);
            builder.Services.AddSingleton<IQueryDispatcher<Post>>(_ => dispatcher);

            builder.Services.AddControllers();
            builder.Services.AddHostedService<ConsumerHostedService>();

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

        // For Demo purposes - Create database if not exists
        public static void CreateDatabaseIfNotExists(this WebApplication app)
        {
            using (IServiceScope scope = app.Services.CreateScope())
            {
                IServiceProvider service = scope.ServiceProvider;

                try
                {
                    DatabaseContext dbContext = service.GetRequiredService<DatabaseContext>();

                    dbContext?.Database.EnsureCreated();
                }
                catch (Exception ex)
                {
                    ILogger logger = scope.ServiceProvider.GetRequiredService<ILogger<Program>>();
                    logger.LogError(ex, "An Error occurred while creating the database.");
                }
            }
        }
    }
}