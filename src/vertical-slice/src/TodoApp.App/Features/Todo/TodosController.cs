namespace TodoApp.App.Features.Todo;

[Route("api/normal/todos")]
public class TodosController : ControllerBase
{
    private readonly IMediator _mediator;

    public TodosController(IMediator mediator)
    {
        _mediator = mediator;
    }

    // [HttpGet]
    // [ProducesResponseType(typeof(IEnumerable<GetTodos.Response>), StatusCodes.Status200OK)]
    // [ProducesResponseType(StatusCodes.Status400BadRequest)]
    // public async Task<IActionResult> GetAllTodo()
    // {
    //     var result = await _mediator.Send(new GetTodos.Query());
    //     return Ok(result);
    // }
}