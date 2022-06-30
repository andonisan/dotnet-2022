using TodoApp.App.Common.Behaviors;

namespace TodoApp.App.Features.Todo.Commands;

public class CreateTodo : IFeatureModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPost("/todos",
                async (Command command, IMediator mediator, CancellationToken cancellationToken) =>
                {
                    await mediator.Send(command, cancellationToken);

                    return Results.Ok();
                })
            .WithName(nameof(CreateTodo))
            .WithTags(TodoConstants.TodosFeature)
            .Produces(StatusCodes.Status200OK)
            .Produces<ProblemDetails>(StatusCodes.Status409Conflict);
    }

    public class Command : ICommand
    {
       public string Title { get; set; } 
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

            var todo = new Domain.Entities.Todo(request.Title);

            await _db.Todos.AddAsync(todo, cancellationToken);

            await _db.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
    }

    public class Validator : AbstractValidator<Command>
    {
        public Validator()
        {
            RuleFor(r => r.Title).NotEmpty().MinimumLength(10);
        }
    }
}