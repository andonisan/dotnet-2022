using TodoApp.App.Common.Behaviors;

namespace TodoApp.App.Features.Todo.Queries;

public class GetTodos : IFeatureModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet("/todos",
                async (IMediator mediator, CancellationToken cancellationToken) =>
                {
                    return  await mediator.Send(new Query(), cancellationToken);
                })
            .WithName(nameof(GetTodos))
            .WithTags(TodoConstants.TodosFeature)
            .Produces<Response[]>();
    }

    public record Query : IQuery<Response[]>, ICacheRequest
    {
        public string CacheKey => TodoConstants.CachePrefix;
        public DateTime? AbsoluteExpirationRelativeToNow { get; }
    }

    public record Response(string Id, string Caption, bool IsCompleted);

    public class RequestHandler : IRequestHandler<Query, Response[]>
    {
        private readonly ReadOnlyTodoDbContext _db;

        public RequestHandler(ReadOnlyTodoDbContext db)
        {
            _db = db;
        }

        public async Task<Response[]> Handle(Query request, CancellationToken cancellationToken = default)
        {
            return await _db.Todos
                .Select(td => new Response(td.Id, td.Title, td.Completed))
                .ToArrayAsync(cancellationToken);
        }
    }
    
}