namespace TodoApp.App.Domain.Entities;

public class WorkAssignment : BaseEntity
{
    protected WorkAssignment()
    {
        
    }

    public WorkAssignment(Developer developer, Todo todo, int daysEffort)
    {
        Developer = developer;
        Todo = todo;
        DaysEffort = daysEffort;
        StartDateExpected = developer.WorkingType.GetStartDateExpected();
    }

    public Developer Developer { get; private set; }
    public Todo Todo { get; private set; }
    public int DaysEffort { get; private set; }
    public DateTime StartDateExpected { get; private set; }
}