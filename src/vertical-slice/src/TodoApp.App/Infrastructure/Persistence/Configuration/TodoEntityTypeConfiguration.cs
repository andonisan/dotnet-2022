using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace TodoApp.App.Infrastructure.Persistence.Configuration
{
    internal class TodoEntityTypeConfiguration
        : IEntityTypeConfiguration<Todo>
    {
        public void Configure(EntityTypeBuilder<Todo> builder)
        {
            builder.ToTable("todos");
          
            builder.HasKey(t => t.Id);
           
            builder.Property(t => t.Title)
                .IsRequired();

            builder.Ignore(t => t.DomainEvents);
          
            builder.Property(t => t.Completed)
                .HasColumnType("BIT")
                .IsRequired();
        }
    }
}
