using Microsoft.EntityFrameworkCore;
using TodoApp.App.Domain.Entities;
using TodoApp.App.Persistence.Configurations;

namespace TodoApp.App.Persistence;

public class TodoDbContext : DbContext
{
    public TodoDbContext(DbContextOptions options) : base(options)
    {
            
    }
        
    public DbSet<Todo> Todos { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new TodoEntityConfiguration());
        base.OnModelCreating(modelBuilder);
    }
}