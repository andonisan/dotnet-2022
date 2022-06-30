using System.Data.Common;

namespace TodoApp.App.Infrastructure.Persistence;

public sealed class ReadOnlyTodoDbContext : TodoDbContext
{
    public ReadOnlyTodoDbContext(DbConnection connection) : base(connection, null!)
    {
        ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;
    }

    [Obsolete("This context is read-only", true)]
    public new int SaveChanges()
    {
        throw new InvalidOperationException("This context is read-only.");
    }

    [Obsolete("This context is read-only", true)]
    public new int SaveChanges(bool acceptAll)
    {
        throw new InvalidOperationException("This context is read-only.");
    }

    [Obsolete("This context is read-only", true)]
    public new Task<int> SaveChangesAsync(CancellationToken token = default)
    {
        throw new InvalidOperationException("This context is read-only.");
    }

    [Obsolete("This context is read-only", true)]
    public new Task<int> SaveChangesAsync(bool acceptAll, CancellationToken token = default)
    {
        throw new InvalidOperationException("This context is read-only.");
    }
}