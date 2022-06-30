using MediatR;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Respawn;
using TodoApp.App.Infrastructure.Persistence;
using TodoApp.App.IntegrationEventsModule.Persistence;
using TodoApp.Host;

namespace FunctionalTests.Seedwork;

public class SliceFixture : IAsyncLifetime
{
    private static Checkpoint _checkpoint;
    private static IConfiguration _configuration;
    private readonly IServiceScopeFactory _scopeFactory;
    private readonly WebApplicationFactory<Startup> _factory;

    public HttpClient Client { get; set; }

    public SliceFixture()
    {
        _factory = new TestApplicationFactory();

        _configuration = _factory.Services.GetRequiredService<IConfiguration>();
        _scopeFactory = _factory.Services.GetRequiredService<IServiceScopeFactory>();

        _checkpoint = new Checkpoint();
        Client = _factory.CreateClient();
    }

    class TestApplicationFactory : WebApplicationFactory<Startup>
    {
        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.ConfigureAppConfiguration((configBuilder) =>
            {
                configBuilder
                    .SetBasePath(Directory.GetCurrentDirectory())
                    .AddJsonFile("appsettings.json")
                    .AddEnvironmentVariables();
            });
        }
    }

    public async Task ExecuteScopeAsync<TContext>(Func<IServiceProvider, Task> action) where TContext : DbContext
    {
        using var scope = _scopeFactory.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<TContext>();
        await action(scope.ServiceProvider);
    }
    
    public Task ExecuteDbContextAsync<T>(Func<T, Task> action) where T : DbContext =>
        ExecuteScopeAsync<T>(sp => action(sp.GetService<T>()));

    public Task InsertAsync<T, TContext>(params T[] entities) where T : class where TContext : DbContext
    {
        return ExecuteDbContextAsync<TContext>(db =>
        {
            foreach (var entity in entities)
            {
                db.Set<T>().Add(entity);
            }

            return db.SaveChangesAsync();
        });
    }

    public Task InsertAsync<TEntity, TContext>(TEntity entity) where TEntity : class where TContext : DbContext
    {
        return ExecuteDbContextAsync<TContext>(db =>
        {
            db.Set<TEntity>().Add(entity);

            return db.SaveChangesAsync();
        });
    }

    public async Task InitializeAsync()
    {
        await MigrateAsync<TodoDbContext>();
        await MigrateAsync<IntegrationEventContext>();
    }
    
    private async Task MigrateAsync<TContext>() where TContext : DbContext
    {
        using var scope = _scopeFactory.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<TContext>();
        await dbContext.Database.MigrateAsync();
    }
    
    private async Task DropDatabaseAsync<TContext>() where TContext : DbContext
    {
        using var scope = _scopeFactory.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<TContext>();
        await dbContext.Database.EnsureDeletedAsync();
    }

    public async Task DisposeAsync()
    {
        await DropDatabaseAsync<TodoDbContext>();
        _factory?.Dispose();
    }

    public static void ResetDatabase()
    {
        _checkpoint.Reset(_configuration.GetConnectionString("SqlServer"));
    }
}