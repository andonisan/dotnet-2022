using Microsoft.EntityFrameworkCore;
using TodoApp.App.Infrastructure.Persistence;

namespace TodoApp.Host.DependencyInjection
{
    internal static class WebApplicationExtensions
    {
     public static async Task MigrateDbContext<TContext>(this WebApplication webApplication)
            where TContext : DbContext
        {
            var scopeFactory = webApplication.Services.GetRequiredService<IServiceScopeFactory>();

            using var scope = scopeFactory.CreateScope();
            
            var db = scope.ServiceProvider
                .GetRequiredService<TodoDbContext>();

            await db.Database.MigrateAsync();
        }
    }
}
