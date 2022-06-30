using FluentValidation.AspNetCore;
using Hellang.Middleware.ProblemDetails;
using Microsoft.Extensions.DependencyInjection;
using TodoApp.App.Common.Behaviors;
using TodoApp.App.IntegrationEventsModule;
using TodoApp.App.Services;

namespace TodoApp.App.DependencyInjection
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddAppServices(this IServiceCollection services,
            IConfiguration configuration)
        {
            services.AddControllers()
                .AddFluentValidation(fv => fv.RegisterValidatorsFromAssemblyContaining<App>())
                .AddApplicationPart(typeof(App).Assembly);

            services.AddCustomProblemDetails()
                .AddValidatorsFromAssembly(typeof(App).Assembly)
                .AddDiagnostics(configuration)
                .AddMediatR(typeof(App).Assembly);

            services.AddTransient<IEffotCalculator, EffotCalculator>();

            //   services.AddBehaviors()
            //    .AddEasyCaching(options => { options.UseInMemory(Cache.CacheDefaultName); });

            //  services.AddIntegrationEvents();

            return services;
        }


        static IServiceCollection AddBehaviors(this IServiceCollection services)
        {
            return services
                .AddTransient(typeof(IPipelineBehavior<,>), typeof(LoggingBehavior<,>))
                // .AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>))
                .AddTransient(typeof(IPipelineBehavior<,>), typeof(TimeLoggingBehavior<,>))
                .AddTransient(typeof(IPipelineBehavior<,>), typeof(CachingBehavior<,>))
                .AddTransient(typeof(IPipelineBehavior<,>), typeof(InvalidateCachingBehavior<,>));
        }

        static IServiceCollection AddDiagnostics(this IServiceCollection services, IConfiguration configuration)
        {
            return services.AddApplicationInsightsTelemetry(configuration);
        }

        static IServiceCollection AddCustomProblemDetails(this IServiceCollection services)
        {
            return services.AddProblemDetails(setup =>
            {
                setup.Map<InvalidOperationException>(exception =>
                    new StatusCodeProblemDetails(StatusCodes.Status409Conflict)
                    {
                        Detail = exception.Message
                    });
                setup.Map<ValidationException>(exception =>
                    new StatusCodeProblemDetails(StatusCodes.Status409Conflict)
                    {
                        Detail = exception.Message
                    });
                setup.MapToStatusCode<NotImplementedException>(StatusCodes.Status501NotImplemented);
                setup.MapToStatusCode<HttpRequestException>(StatusCodes.Status503ServiceUnavailable);
                setup.MapToStatusCode<Exception>(StatusCodes.Status500InternalServerError);
            });
        }
    }
}