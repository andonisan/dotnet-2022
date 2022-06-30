using System.Net.Http.Json;

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
        private readonly HttpClient _httpClient;

        public Handler(TodoDbContext dbContext,
            HttpClient httpClient)
        {
            _dbContext = dbContext;
            _httpClient = httpClient;
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

            // MAGIC AND VERY EXPENSIVE External estimator
            var response = await _httpClient
                .GetAsync(requestUri: $"/external-api/estimate-work?title={todo.Title}&developer={developer.Mail}",
                    cancellationToken: cancellationToken);
            
            response.EnsureSuccessStatusCode();
            
            var daysEffort = await response.Content
                .ReadFromJsonAsync<int>(cancellationToken: cancellationToken);

            // Calculate start expected date
            DateTime startDateExpected;

            switch (developer.WorkingType)
            {
                case WorkingType.Fast:
                    startDateExpected = DateTime.Today;
                    break;
                case WorkingType.Calm:
                    startDateExpected = DateTime.Today.AddDays(1);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            // Assignment
            var assignment = new WorkAssignment(
                developer,
                todo,
                daysEffort,
                startDateExpected);

            developer.WorkAssignments.Add(assignment);
            developer.NumberOfActiveAssignments++;

            await _dbContext.SaveChangesAsync(cancellationToken);
            return Unit.Value;
        }
    }
}