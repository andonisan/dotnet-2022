using TodoApp.App.Common.Behaviors;

namespace TodoApp.App.Features.Todo.Queries;

public class GetTodo : IFeatureModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet("/todos/{id}",
                async (string id, IMediator mediator, CancellationToken cancellationToken) =>
                {
                    var request = new Query(id);

                    var response = await mediator.Send(request, cancellationToken);

                    return response != null ? Results.Ok(response) : Results.NotFound();
                })
            .WithName(nameof(GetTodo))
            .WithTags(TodoConstants.TodosFeature)
            .Produces<Response>()
            .Produces<ProblemDetails>(StatusCodes.Status400BadRequest)
            .Produces(StatusCodes.Status404NotFound);
    }

    public record Query(string TodoId) : IQuery<Response>, ICacheRequest
    {
        public string CacheKey => $"{TodoConstants.CachePrefix}_{TodoId}";
        public DateTime? AbsoluteExpirationRelativeToNow { get; }
    }

    public record Response(string Id, string Caption, bool IsCompleted);

    public class RequestHandler : IRequestHandler<Query, Response?>
    {
        private readonly ReadOnlyTodoDbContext _db;
        private readonly IValidator<Query> _validator;

        public RequestHandler(ReadOnlyTodoDbContext db, IValidator<Query> validator)
        {
            _db = db;
            _validator = validator;
        }

        public async Task<Response?> Handle(Query request, CancellationToken cancellationToken = default)
        {
            var result = await _validator.ValidateAsync(request, cancellationToken);

            if (!result.IsValid)
            {
                throw new InvalidOperationException(result.ToString());
            }

            return await _db.Todos
                .AsNoTracking()
                .Where(t => t.Id == request.TodoId)
                .Select(td => new Response(td.Id, td.Title, td.Completed))
                .SingleOrDefaultAsync(cancellationToken);
        }
    }

    public class RequestValidator : AbstractValidator<Query>
    {
        public RequestValidator()
        {
            RuleFor(r => r.TodoId).NotEmpty();
        }
    }
}