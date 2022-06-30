using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore.Design;

namespace TodoApp.App.Infrastructure.Persistence
{
    internal class TodoDbContextDesignTimeFactory
        : IDesignTimeDbContextFactory<TodoDbContext>
    {
        public TodoDbContext CreateDbContext(string[] args)
        {
            // just used it to add migrations on this project because is not 
            // a executable, is not used on runtime

            var options = new DbContextOptionsBuilder<TodoDbContext>();
            return new TodoDbContext(new SqlConnection("Server=(localdb)\\mssqllocaldb;Database=PlainTodoVerticalSlice"), null!);
        }
    }
}
