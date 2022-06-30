using TodoApp.App.Infrastructure.Persistence;
using TodoApp.App.IntegrationEventsModule.Persistence;
using TodoApp.Host;
using TodoApp.Host.DependencyInjection;

var builder = WebApplication.CreateBuilder(args);

builder.Host.AddSerilog(builder.Configuration);

var startup = new Startup(builder.Configuration);

startup.ConfigureServices(builder.Services);

var app = builder.Build();

startup.Configure(app, app.Environment);

await app.MigrateDbContext<TodoDbContext>();
await app.MigrateDbContext<IntegrationEventContext>();

app.Run();
