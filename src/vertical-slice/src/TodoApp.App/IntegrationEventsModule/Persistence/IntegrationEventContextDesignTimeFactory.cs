using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore.Design;

namespace TodoApp.App.IntegrationEventsModule.Persistence;

internal class IntegrationEventContextDesignTimeFactory
    : IDesignTimeDbContextFactory<IntegrationEventContext>
{
    public IntegrationEventContext CreateDbContext(string[] args)
    {
        // just used it to add migrations on this project because is not 
        // a executable, is not used on runtime

        var builder = new DbContextOptionsBuilder<IntegrationEventContext>();
        
        return new IntegrationEventContext(new SqlConnection("Server=(localdb)\\mssqllocaldb;Database=PlainTodoVerticalSlice"));
    }
}