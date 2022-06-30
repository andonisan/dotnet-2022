using TodoApp.App.IntegrationEventsModule.Domain;

namespace TodoApp.App.Domain.Events;

public class TodoCompletedEvent : IntegrationEvent, INotification
{
    public string TodoId { get; }

    public TodoCompletedEvent(string todoId)
    {
        TodoId = todoId;
    }
}