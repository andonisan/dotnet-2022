namespace TodoApp.App.Domain.Entities;

public class Todo : BaseEntity
{
    public string Title { get; private set; }
        
    public bool Completed { get; private set; }

    public Todo(string title)
    {
        Title = title;
    }

    public void UpdateTitle(string title)
    {
        ArgumentNullException.ThrowIfNull(title);
        Title = title;
    }
    
    public void MarkComplete()
    {
        if (!Completed)
        {
            Completed = true;
        }
    }
}