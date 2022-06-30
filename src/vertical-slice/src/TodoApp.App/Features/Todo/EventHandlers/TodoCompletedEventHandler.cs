using ColoredConsole;
using TodoApp.App.Domain.Events;
using TodoApp.App.IntegrationEventsModule.Services;

namespace TodoApp.App.Features.Todo.EventHandlers;

public class TodoCompletedEventHandler : INotificationHandler<TodoCompletedEvent>
{
    private readonly IIntegrationEventService _integrationEventService;
    
    public TodoCompletedEventHandler(IIntegrationEventService integrationEventService)
    {
        _integrationEventService = integrationEventService;
    }

    public async Task Handle(TodoCompletedEvent notification, CancellationToken cancellationToken)
    {
        ColorConsole.WriteLine($"Completed Domain Event logged with todoId {notification.TodoId} to be sent to the integration event service".Green());
      
        await _integrationEventService.AddEventAsync(notification, cancellationToken);
    }
}


