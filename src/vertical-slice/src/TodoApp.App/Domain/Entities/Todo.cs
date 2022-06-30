using TodoApp.App.Domain.Events;

namespace TodoApp.App.Domain.Entities
{
    public class Todo : BaseEntity
    {
        public string Title { get; private set; }
        
        public bool Completed { get; private set; }
        
        public Todo(string title)
        {
            if (string.IsNullOrEmpty(title))
            {
                throw new ArgumentNullException(nameof(title));
            }
            Title = title;
        }

        public void UpdateTitle(string title)
        {
            if (string.IsNullOrEmpty(title))
            {
                throw new ArgumentNullException(nameof(title));
            }
            Title = title;
        }

        public void CompleteTodo()
        {
            if (Completed == false)
            {
                Completed = true;
                AddDomainEvent(new TodoCompletedEvent(Id));
            }
        }
    }
}