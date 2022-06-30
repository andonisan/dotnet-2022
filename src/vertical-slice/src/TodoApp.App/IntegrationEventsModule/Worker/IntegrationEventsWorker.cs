using ColoredConsole;
using MassTransit;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using TodoApp.App.IntegrationEventsModule.Domain;
using TodoApp.App.IntegrationEventsModule.Persistence;

namespace TodoApp.App.IntegrationEventsModule.Worker;

public interface ValueEntered
{
    string Value { get; }
}

public class IntegrationEventsWorker : BackgroundService
{
    private readonly IServiceProvider _serviceProvider;

    public IntegrationEventsWorker(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    protected override async Task ExecuteAsync(CancellationToken cancellationToken)
    {
        var busControl = Bus.Factory.CreateUsingRabbitMq();

        await busControl.StartAsync(cancellationToken);

        try
        {
            while (!cancellationToken.IsCancellationRequested)
            {
                using var scope = _serviceProvider.CreateScope();

                await using var context = scope.ServiceProvider.GetRequiredService<IntegrationEventContext>();

                var events = await context.IntegrationEvents
                    .Where(e => e.State == EventStateEnum.NotPublished)
                    .ToArrayAsync(cancellationToken);

                if (events.Any())
                {
                    foreach (var integrationEventEntry in events)
                    {
                        ColorConsole.WriteLine(
                            $"Publishing integration event to event bus: {integrationEventEntry.EventId}".Green());
                        ColorConsole.WriteLine(integrationEventEntry.Content.Yellow());

                        integrationEventEntry.SetState(EventStateEnum.Published);

                        try
                        {
                            var endpoint = await busControl.GetSendEndpoint(new Uri("queue:todo-service"));
                            await endpoint.Send(integrationEventEntry.OriginalIntegrationEvent, cancellationToken);
                            await context.SaveChangesAsync(cancellationToken);
                        }
                        catch (Exception e)
                        {
                            ColorConsole.WriteLine(e.Message.DarkRed());
                        }
                    }
                }

                await Task.Delay(5000, cancellationToken);
            }
        }
        finally
        {
            await busControl.StopAsync();
        }
    }
}