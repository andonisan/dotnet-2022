using System.Data.Common;
using TodoApp.App.IntegrationEventsModule.Domain;
using TodoApp.App.IntegrationEventsModule.Persistence.Configurations;

namespace TodoApp.App.IntegrationEventsModule.Persistence
{
    public class IntegrationEventContext : DbContext
    {
        private readonly DbConnection _connection;

        public DbSet<IntegrationEventEntry> IntegrationEvents => Set<IntegrationEventEntry>();

        public IntegrationEventContext(DbConnection connection)
        {
            _connection = connection;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(_connection,
                options => { options.MigrationsHistoryTable("__IntegrationEventsMigrationsHistory"); });
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.HasDefaultSchema("events");

            modelBuilder.ApplyConfiguration(new IntegrationEventEntryConfiguration());
        }
    }
}