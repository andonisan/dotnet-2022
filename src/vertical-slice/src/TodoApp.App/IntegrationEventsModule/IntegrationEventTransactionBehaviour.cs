using Microsoft.EntityFrameworkCore.Storage;
using TodoApp.App.IntegrationEventsModule.Services;

namespace TodoApp.App.IntegrationEventsModule
{
    public class IntegrationEventTransactionBehaviour<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
        where TRequest : IRequest<TResponse>
    {
        private readonly IIntegrationEventService _integrationEventService;
        private readonly ILogger<IntegrationEventTransactionBehaviour<TRequest, TResponse>> _logger;
        private readonly TodoDbContext _context;

        public IntegrationEventTransactionBehaviour(
            ILogger<IntegrationEventTransactionBehaviour<TRequest, TResponse>> logger,
            IIntegrationEventService integrationEventService,
            TodoDbContext context)
        {
            _logger = logger;
            _integrationEventService = integrationEventService;
            _context = context;
        }

        public async Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken,
            RequestHandlerDelegate<TResponse> next)
        {
            if (request is IBaseQuery)
            {
                return await next();
            }
            
            await using var transaction = await _context.Database.BeginTransactionAsync(cancellationToken);
            try
            {
                _logger.LogInformation("Begin transaction : {Transaction}", transaction.TransactionId);

                var response = await next();

                await _integrationEventService.SaveEventsAsync(transaction.GetDbTransaction(), cancellationToken);
                
                await transaction.CommitAsync(cancellationToken);

                _logger.LogInformation("End transaction : {Transaction}", transaction.TransactionId);
                
                return response;
            }
            catch (Exception)
            {
                _logger.LogInformation("Failed transaction : {Transaction}", transaction.TransactionId);
                await transaction.RollbackAsync(cancellationToken);
                throw;
            }
        }
    }
}