using FluentValidation.AspNetCore;
using Hellang.Middleware.ProblemDetails;
using Microsoft.AspNetCore.Mvc;
using TodoApp.App.Domain;
using TodoApp.App.Repositories;
using TodoApp.App.Services;
using TodoApp.App.Services.Interfaces;

namespace TodoApp.Host.DependencyInjection;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddHttpApiServices(this IServiceCollection services, IConfiguration configuration)
    {
        // added mandatory services for this project 

        services.AddControllers()
            .AddFluentValidation(fv => fv.RegisterValidatorsFromAssemblyContaining<Host>())
            .AddApplicationPart(typeof(Host).Assembly)
            .Services
            .AddProblemDetails(setup =>
            {
                setup.Map<DomainException>(exception =>
                {
                    var details = new ProblemDetails
                    {
                        Title = exception.Title,
                        Status = StatusCodes.Status400BadRequest,
                        Detail = exception.Description,
                        Type = exception.GetType().Name
                    };

                    return details;
                });
            });

        services
            .AddDiagnostics(configuration);

        services.AddScoped<ITodoRepository,TodoRepository>();
        services.AddTransient<ITodoService, TodoService>();

        return services;
    }

    static IServiceCollection AddDiagnostics(this IServiceCollection services, IConfiguration configuration)
    {
        return services.AddApplicationInsightsTelemetry(configuration);
    }

    public static IServiceCollection AddOpenApi(this IServiceCollection services)
    {
        return services
            .AddEndpointsApiExplorer()
            .AddSwaggerGen(options => { options.CustomSchemaIds(type => type.ToString()); });
    }
}