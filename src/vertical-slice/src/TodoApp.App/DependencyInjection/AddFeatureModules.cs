namespace TodoApp.App.DependencyInjection;

public static class AddFeatureModules
{
    public static void MapFeatures(this IEndpointRouteBuilder builder)
    {
        var features = typeof(IFeatureModule).Assembly
            .GetTypes()
            .Where(p => p.IsClass && p.IsPublic && p.IsAssignableTo(typeof(IFeatureModule)))
            .Select(Activator.CreateInstance)
            .Cast<IFeatureModule>();

        foreach (var feature in features)
        {
            feature.AddRoutes(builder);
        }
    }
}