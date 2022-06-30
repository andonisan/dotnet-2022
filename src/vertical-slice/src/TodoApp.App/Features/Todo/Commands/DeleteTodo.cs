namespace TodoApp.App.Features.Todo.Commands;

public class DeleteTodo  : IFeatureModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapDelete("/todos/{id}",
                async (string  id, IMediator mediator, CancellationToken cancellationToken) =>
                {
                    var request = new Command()
                    {
                        TodoId = id
                    };
                    
                    await mediator.Send(request, cancellationToken);

                    return Results.Ok();
                })
            .WithName(nameof(DeleteTodo))
            .WithTags(TodoConstants.TodosFeature)
            .Produces(StatusCodes.Status200OK)
            .Produces<ProblemDetails>(StatusCodes.Status400BadRequest)
            .Produces(StatusCodes.Status404NotFound);
    }
    
   public class Command : ICommand
   {
       public string TodoId { get; set; } = string.Empty;
   }

    public class CommandHandler : IRequestHandler<Command>
    {
        private readonly TodoDbContext _db;
        private readonly IValidator<Command> _validator;

        public CommandHandler(TodoDbContext db, IValidator<Command> validator)
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
                return Unit.Value;
            }

            _db.Todos.Remove(todo);
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