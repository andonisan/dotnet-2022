using System.Data.Common;
using Microsoft.EntityFrameworkCore.Storage;
using TodoApp.App.IntegrationEventsModule.Domain;
using TodoApp.App.IntegrationEventsModule.Persistence;

namespace TodoApp.App.IntegrationEventsModule.Services
{
    public class IntegrationEventService : IIntegrationEventService
    {
        private readonly IntegrationEventContext _context;

        public IntegrationEventService(IntegrationEventContext context)
        {
            _context = context;
        }

        public async Task AddEventAsync(IntegrationEvent @event, CancellationToken cancellationToken)
        {
            var eventLogEntry = new IntegrationEventEntry(@event);
            await _context.IntegrationEvents.AddAsync(eventLogEntry, cancellationToken);
        }

        public Task MarkEventAsPublishedAsync(Guid eventId)
        {
            return UpdateEventStatus(eventId, EventStateEnum.Published);
        }

        public Task MarkEventAsInProgressAsync(Guid eventId)
        {
            return UpdateEventStatus(eventId, EventStateEnum.InProgress);
        }

        public Task MarkEventAsFailedAsync(Guid eventId)
        {
            return UpdateEventStatus(eventId, EventStateEnum.PublishedFailed);
        }

        private bool HasToStoreEvent()
        {
            return _context.ChangeTracker
                .Entries<IntegrationEventEntry>()
                .Any(e => e.State == EntityState.Added);
        }

        public async Task SaveEventsAsync(DbTransaction dbTransaction,
            CancellationToken cancellationToken)
        {
            if (HasToStoreEvent())
            {
                
                await _context.Database.UseTransactionAsync(dbTransaction, cancellationToken);

                await _context.SaveChangesAsync(cancellationToken);
            }
        }

        private Task UpdateEventStatus(Guid eventId, EventStateEnum status)
        {
            var eventLogEntry = _context.IntegrationEvents.Single(ie => ie.EventId == eventId);
            
            eventLogEntry.SetState(status);

            _context.IntegrationEvents.Update(eventLogEntry);

            return _context.SaveChangesAsync();
        }
    }
}