namespace TodoApp.App.Domain.Entities;

public class Developer : BaseEntity
{
    protected Developer()
    {
    }
    
    public Developer(int actualWork, DateTime? beginDate, WorkingType workingType, string mail)
    {
        ActualWork = actualWork;
        BeginDate = beginDate;
        WorkingType = workingType;
        Mail = mail;
    }

    public string Mail { get; private set; } 
    public int ActualWork { get; private set; }
    public DateTime? BeginDate { get; private set; }
    public WorkingType WorkingType { get; private set; }
    public List<WorkAssignment> WorkAssignments { get; set; } = new();
    public int NumberOfActiveAssignments { get; set; }
}