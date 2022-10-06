using DotNet.Testcontainers.Builders;
using DotNet.Testcontainers.Configurations;
using DotNet.Testcontainers.Containers;
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

public class SliceFixture : WebApplicationFactory<Startup>, IAsyncLifetime
{
    private Checkpoint _checkpoint;
    private IServiceScopeFactory _scopeFactory;

    private readonly TestcontainerDatabase _dbContainer = new TestcontainersBuilder<MsSqlTestcontainer>()
        .WithDatabase(new MsSqlTestcontainerConfiguration("mcr.microsoft.com/mssql/server:2017-latest-ubuntu")
        {
            Password = "#testingDockerPassword#",
        })
        .Build();

    public HttpClient Client { get; set; }

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
        await _dbContainer.StartAsync();
        Environment.SetEnvironmentVariable("ConnectionStrings__SqlServer",
            $"{_dbContainer.ConnectionString}TrustServerCertificate=true;");
        _scopeFactory = Services.GetRequiredService<IServiceScopeFactory>();
        _checkpoint = new Checkpoint();
        Client = CreateClient();

        await MigrateAsync<TodoDbContext>();
        await MigrateAsync<IntegrationEventContext>();
    }

    private async Task MigrateAsync<TContext>() where TContext : DbContext
    {
        using var scope = _scopeFactory.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<TContext>();
        await dbContext.Database.MigrateAsync();
    }

    public async Task DisposeAsync()
    {
        await _dbContainer.StopAsync();
        Dispose();
    }

    public void ResetDatabase()
    {
        _checkpoint.Reset(_dbContainer.ConnectionString);
    }
}