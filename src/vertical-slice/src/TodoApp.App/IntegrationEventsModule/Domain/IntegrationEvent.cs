namespace TodoApp.App.IntegrationEventsModule.Domain;

public interface IHasIntegrationEvent
{
    public List<IntegrationEvent> IntegrationEventEvents { get; set; }
}

public abstract class IntegrationEvent
{
    public Guid EventId { get; private set; }

    public DateTimeOffset DateOccurred { get; protected set; }


    protected IntegrationEvent()
    {
        EventId = Guid.NewGuid();
        DateOccurred = DateTimeOffset.UtcNow;
    }
}