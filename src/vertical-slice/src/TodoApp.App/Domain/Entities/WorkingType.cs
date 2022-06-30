using Ardalis.SmartEnum;

namespace TodoApp.App.Domain.Entities;

public abstract class WorkingType : SmartEnum<WorkingType>
{

    public static  WorkingType Fast = new FastWorkingType();
    public static  WorkingType Calm = new CalmWorkingType();
    
    private class FastWorkingType : WorkingType
    {
        public FastWorkingType() : base(nameof(Fast), 1)
        {
        }

        public override DateTime GetStartDateExpected()
        {
            return DateTime.Today;
        }
    }
    
    private class CalmWorkingType : WorkingType
    {
        public CalmWorkingType() : base(nameof(Calm), 2)
        {
        }

        public override DateTime GetStartDateExpected()
        {
            return  DateTime.Today.AddDays(1);
        }
    }

    private WorkingType(string name, int value) : base(name, value)
    {
    }

    public abstract DateTime GetStartDateExpected();
  
}