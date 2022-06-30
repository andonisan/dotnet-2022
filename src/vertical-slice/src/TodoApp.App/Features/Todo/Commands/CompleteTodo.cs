


using TodoApp.App.Domain.Events;

namespace TodoApp.App.Features.Todo.Commands;

public class CompleteTodo : IFeatureModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPut("/todos/{id}/complete",
                async (string id, IMediator mediator, CancellationToken cancellationToken) =>
                {
                    var request = new Command(id);

                    await mediator.Send(request, cancellationToken);

                    return Results.Ok();
                })
            .WithName(nameof(CompleteTodo))
            .WithTags(TodoConstants.TodosFeature)
            .Produces(StatusCodes.Status200OK)
            .Produces<ProblemDetails>(StatusCodes.Status409Conflict)
            .Produces(StatusCodes.Status404NotFound);
    }

    public record Command(string TodoId) : ICommand;

    public class Handler : IRequestHandler<Command>
    {
        private readonly TodoDbContext _db;
        private readonly IValidator<Command> _validator;

        public Handler(TodoDbContext db, IValidator<Command> validator)
        {
            _db = db;
            _validator = validator;
        }

        public async Task<Unit> Handle(Command request, CancellationToken cancellationToken)
        {
            var result = await _validator.ValidateAsync(request, cancellationToken);

            if (!result.IsValid)
            {
                throw new ValidationException(result.Errors);
            }

            var todo = await _db.Todos
                .SingleOrDefaultAsync(x => x.Id == request.TodoId, cancellationToken);

            if (todo == null)
            {
                throw new InvalidOperationException($"Todo {request.TodoId} not found");
            }

            todo.CompleteTodo();

            await _db.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
    }

    public class Validator : AbstractValidator<Command>
    {
        public Validator()
        {
            RuleFor(r => r.TodoId).NotEmpty();
        }
    }
}