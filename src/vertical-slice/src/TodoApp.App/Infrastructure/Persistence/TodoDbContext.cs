using System.Data.Common;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using TodoApp.App.Infrastructure.Persistence.Configuration;
using Microsoft.EntityFrameworkCore.Storage;

namespace TodoApp.App.Infrastructure.Persistence
{
    public class TodoDbContext : DbContext
    {
        private readonly IMediator _mediator;

        public DbSet<Todo> Todos => Set<Todo>();

        public DbSet<Developer> Developers => Set<Developer>();

        private readonly DbConnection _connection;
  
        public TodoDbContext(DbConnection connection, IMediator mediator)
        {
            _connection = connection;
            _mediator = mediator;
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(_connection);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            
            modelBuilder.ApplyConfiguration(new TodoEntityTypeConfiguration());
        }

        public override int SaveChanges()
        {
            DispatchDomainEventsAsync().GetAwaiter().GetResult();

            return base.SaveChanges();
        }

        public override int SaveChanges(bool acceptAllChangesOnSuccess)
        {
            DispatchDomainEventsAsync().GetAwaiter().GetResult();

            return base.SaveChanges(acceptAllChangesOnSuccess);
        }

        override public async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            await DispatchDomainEventsAsync();

            return await base.SaveChangesAsync(cancellationToken);
        }
        
        private async Task DispatchDomainEventsAsync()
        {
            var domainEntities = ChangeTracker
                .Entries<BaseEntity>()
                .Where(x => x.Entity.DomainEvents.Any());

            var entityEntries = domainEntities as EntityEntry<BaseEntity>[] ?? domainEntities.ToArray();

            if (entityEntries.Any())
            {
                var domainEvents = entityEntries
                    .SelectMany(x => x.Entity.DomainEvents)
                    .ToList();

                entityEntries.ToList()
                    .ForEach(entity => entity.Entity.ClearDomainEvents());

                foreach (var domainEvent in domainEvents)
                {
                    await _mediator.Publish(domainEvent);
                }
            }
        }
    }
}