namespace TodoApp.Host.DependencyInjection;

internal static class ServiceCollectionExtensions
{
    public static IServiceCollection AddOpenApi(this IServiceCollection services)
    {
        return services
            .AddEndpointsApiExplorer()
            .AddSwaggerGen(options =>
            {
                options.CustomSchemaIds(s => s.FullName?.Replace("+", "."));
                //options.DocInclusionPredicate((s, description) => description.ActionDescriptor.EndpointMetadata.OfType<IIncludeOpenApi>().Any());
            });
    }
}