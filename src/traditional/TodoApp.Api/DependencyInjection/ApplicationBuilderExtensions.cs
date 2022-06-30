namespace TodoApp.Host.DependencyInjection;

public static class ApplicationBuilderExtensions
{
    public static void UseOpenApi(this IApplicationBuilder app)
    {
        app.UseSwagger();
        app.UseSwaggerUI();
    }
}