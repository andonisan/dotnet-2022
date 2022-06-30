using HealthChecks.UI.Client;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;

namespace TodoApp.Host.DependencyInjection
{
    internal static class EndpointRouteBuilderExtensions
    {
        public static void MapHealthChecks(this IEndpointRouteBuilder endpointRouteBuilder)
        {
            endpointRouteBuilder.MapHealthChecks("/readiness", new HealthCheckOptions()
            {
                Predicate = _ => true,
                AllowCachingResponses = true,
                ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
            });

            endpointRouteBuilder.MapHealthChecks("/liveness", new HealthCheckOptions()
            {
                Predicate = registration => registration.Name.Equals("self",StringComparison.InvariantCultureIgnoreCase),
                AllowCachingResponses = true
            });
        }
    }
}
