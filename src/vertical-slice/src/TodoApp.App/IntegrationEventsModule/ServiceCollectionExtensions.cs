using Microsoft.Extensions.DependencyInjection;
using TodoApp.App.IntegrationEventsModule.Services;
using TodoApp.App.IntegrationEventsModule.Worker;

namespace TodoApp.App.IntegrationEventsModule
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddIntegrationEvents(this IServiceCollection services)
        {
            services.AddHostedService<IntegrationEventsWorker>()
                .AddScoped<IIntegrationEventService, IntegrationEventService>()
                .AddTransient(typeof(IPipelineBehavior<,>), typeof(IntegrationEventTransactionBehaviour<,>));
            return services;
        }
    }
}