using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TodoApp.App.IntegrationEventsModule.Domain;

namespace TodoApp.App.IntegrationEventsModule.Persistence.Configurations;

    internal class IntegrationEventEntryConfiguration : IEntityTypeConfiguration<IntegrationEventEntry>
    {
        public void Configure(EntityTypeBuilder<IntegrationEventEntry> builder)
        {
            builder.ToTable("integrationEvents");

            builder.HasKey(e => e.EventId);

            builder.Property(e => e.EventId)
                .IsRequired();

            builder.Property(e => e.Content)
                .IsRequired();

            builder.Property(e => e.DateOccurred)
                .IsRequired();

            builder.Property(e => e.State)
                .IsRequired();

            builder.Property(e => e.TimesSent)
                .IsRequired();

            builder.Property(e => e.EventTypeName)
                .IsRequired();
        }
    }
