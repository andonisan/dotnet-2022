using System.Text.Json;

namespace TodoApp.App.IntegrationEventsModule.Domain
{
    public class IntegrationEventEntry
    {
        private IntegrationEventEntry()
        {
        }

        public IntegrationEventEntry(IntegrationEvent @event)
        {
            EventId = @event.EventId;
            DateOccurred = @event.DateOccurred;
            EventTypeName = @event.GetType().AssemblyQualifiedName;
            Content = JsonSerializer.Serialize<object>(@event);
            State = EventStateEnum.NotPublished;
            TimesSent = 0;
        }

        public Guid EventId { get; private set; }
        public string EventTypeName { get; private set; }
        public EventStateEnum State { get; private set; }
        public int TimesSent { get; private set; }
        public DateTimeOffset DateOccurred { get; private set; }
        public string Content { get; private set; }
        
        public void SetState(EventStateEnum state)
        {
            State = state;
            
            if(state == EventStateEnum.Published)
            {
                TimesSent++;
            }
        }

        public object OriginalIntegrationEvent
        {
            get
            {
                var type = Type.GetType(EventTypeName);
                var content = JsonSerializer.Deserialize(Content, type);
                return content;
            }
        }
    }
}