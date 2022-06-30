using Serilog;
using Serilog.Events;
using Serilog.Sinks.SystemConsole.Themes;

namespace TodoApp.Host.DependencyInjection;

public static class HostBuilderExtensions
{
    public static void AddSerilog(this ConfigureHostBuilder host, IConfiguration configuration)
    {
        host.UseSerilog();

        Log.Logger = new LoggerConfiguration()
            .MinimumLevel.Override("Microsoft", LogEventLevel.Information)
            .Enrich.FromLogContext()
            .WriteTo.Console(theme: AnsiConsoleTheme.Code, outputTemplate: "[{Timestamp:HH:mm:ss} {Level}] {Message}{NewLine}{Exception}")
            .CreateLogger();
    }
}