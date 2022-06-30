using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TodoApp.App.Domain.Entities;

namespace TodoApp.App.Persistence.Configurations;

public class TodoEntityConfiguration : IEntityTypeConfiguration<Todo>
{
    public void Configure(EntityTypeBuilder<Todo> builder)
    {
        builder.ToTable("todos");
          
        builder.HasKey(t => t.Id);
           
        builder.Property(t => t.Title)
            .IsRequired();
          
        builder.Property(t => t.Completed)
            .HasColumnType("BIT")
            .IsRequired();
    }
}