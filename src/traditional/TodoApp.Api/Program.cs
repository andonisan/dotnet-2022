using TodoApp.App.Persistence;
using TodoApp.Host;
using TodoApp.Host.DependencyInjection;

var builder = WebApplication.CreateBuilder(args);

var startup = new Startup(builder.Configuration);

startup.ConfigureServices(builder.Services);

var app = builder.Build();

startup.Configure(app, app.Environment);

await app.MigrateDbContext<TodoDbContext>();

app.Run();
