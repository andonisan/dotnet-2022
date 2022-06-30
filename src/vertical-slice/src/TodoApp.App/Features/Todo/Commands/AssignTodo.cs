using System.Net.Http.Json;
using TodoApp.App.Services;

namespace TodoApp.App.Features.Todo.Commands;

public class AssignTodo
{
    public class Command : ICommand
    {
        public string TodoId { get; set; } = null!;
        public string DeveloperId { get; set; } = null!;
    }

    public class Handler : IRequestHandler<Command>
    {
        private readonly TodoDbContext _dbContext;
        private readonly IEffotCalculator _effotCalculator;

        public Handler(TodoDbContext dbContext, IEffotCalculator effotCalculator)
        {
            _dbContext = dbContext;
            _effotCalculator = effotCalculator;
        }

        public async Task<Unit> Handle(Command request, CancellationToken cancellationToken)
        {
            var todo = await _dbContext.Todos.FindAsync(request.TodoId, cancellationToken);
            if (todo == null)
            {
                throw new InvalidOperationException(nameof(todo));
            }

            var developer = await _dbContext.Developers.FindAsync(request.DeveloperId, cancellationToken);
            if (developer == null)
            {
                throw new InvalidOperationException(nameof(developer));
            }

            var daysEffort = await _effotCalculator.GetDaysEffort(todo, developer, cancellationToken);

            developer.AssignWork(todo, daysEffort);

            await _dbContext.SaveChangesAsync(cancellationToken);
            return Unit.Value;
        }
    }
}