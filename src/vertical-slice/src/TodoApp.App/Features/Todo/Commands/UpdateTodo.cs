using TodoApp.App.Common.Behaviors;

namespace TodoApp.App.Features.Todo.Commands;

public class UpdateTodo : IFeatureModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPut("/todos/{id}",
                async (string id, Command command, IMediator mediator, CancellationToken cancellationToken) =>
                {
                    command.TodoId = id;
                    await mediator.Send(command, cancellationToken);

                    return Results.Ok();
                })
            .WithName(nameof(UpdateTodo))
            .WithTags(TodoConstants.TodosFeature)
            .Produces(StatusCodes.Status200OK)
            .Produces<ProblemDetails>(StatusCodes.Status409Conflict)
            .Produces(StatusCodes.Status404NotFound);
    }

    public class Command : ICommand, IInvalidateCacheRequest
    {
        public string TodoId { get; internal set; } = null!;
        public string Title { get; set; } = null!;
        public string PrefixCacheKey => TodoConstants.CachePrefix;
    }

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

            todo.UpdateTitle(request.Title);

            await _db.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
    }

    public class Validator : AbstractValidator<Command>
    {
        public Validator()
        {
            RuleFor(r => r.TodoId).NotEmpty();
            RuleFor(r => r.Title).NotEmpty();
        }
    }
}