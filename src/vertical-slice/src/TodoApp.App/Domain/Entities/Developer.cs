using System.Collections.ObjectModel;

namespace TodoApp.App.Domain.Entities;

public class Developer : BaseEntity
{
    private List<WorkAssignment> _workAssignments = new();

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

    public ReadOnlyCollection<WorkAssignment> WorkAssignments => _workAssignments.AsReadOnly();

    public int NumberOfActiveAssignments { get; private set; }

    public void AssignWork(Domain.Entities.Todo todo, int daysEffort)
    {
        var assignment = new WorkAssignment(this, todo, daysEffort);

        _workAssignments.Add(assignment);
        NumberOfActiveAssignments++;
    }
}