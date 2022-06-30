using Microsoft.EntityFrameworkCore;
using TodoApp.App.Persistence;

namespace TodoApp.Host.DependencyInjection;

internal static class HostExtensions
{
    public static async Task MigrateDbContext<TContext>(this IHost host)
        where TContext : DbContext
    {
        var scopeFactory = host.Services.GetRequiredService<IServiceScopeFactory>();

        using var scope = scopeFactory.CreateScope();
            
        var db = scope.ServiceProvider
            .GetRequiredService<TodoDbContext>();

        await db.Database.MigrateAsync();
    }
}