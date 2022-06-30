
using System.Data.Common;
using Microsoft.EntityFrameworkCore.Storage;
using TodoApp.App.IntegrationEventsModule.Domain;

namespace TodoApp.App.IntegrationEventsModule.Services
{
    public interface IIntegrationEventService
    {
        Task AddEventAsync(IntegrationEvent @event, CancellationToken cancellationToken);
        Task MarkEventAsPublishedAsync(Guid eventId);
        Task MarkEventAsInProgressAsync(Guid eventId);
        Task MarkEventAsFailedAsync(Guid eventId);
        Task SaveEventsAsync(DbTransaction dbTransaction, CancellationToken cancellationToken);
    }
}
