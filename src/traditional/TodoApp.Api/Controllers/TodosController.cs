using Microsoft.AspNetCore.Mvc;
using TodoApp.App.Dtos.Todos;
using TodoApp.App.Services.Interfaces;

namespace TodoApp.Host.Controllers;

[Route("api/todos")]
public class TodosController : ControllerBase
{
    private readonly ITodoService _todoService;

    public TodosController(ITodoService todoService)
    {
        _todoService = todoService;
    }

    [HttpGet("{id}")]
    [ProducesResponseType(typeof(TodoDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetTodo(string id)
    {
        var result = await _todoService.GetTodo(id);

        if (result is not null)
        {
            return Ok(result);
        }

        return NotFound();
    }

    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<TodoDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> GetAllTodo()
    {
        var result = await _todoService.GetAllTodos();
        return Ok(result);
    }

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> CreateTodo(CreateTodoDto createTodoDto)
    {
        await _todoService.InsertTodo(createTodoDto);
        return Ok();
    }

    [HttpPut("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> UpdateTodo(UpdateTodoDto updateTodoDto, string id)
    {
        updateTodoDto.TodoId = id;
        await _todoService.UpdateTodo(updateTodoDto);
        return Ok();
    }

    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> DeleteTodo(string id)
    {
        await _todoService.DeleteTodo(id);
        return Ok();
    }
    
    [HttpPut("{id}/complete")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> Complete(string id)
    {
        await _todoService.CompleteTodo(id);
        return Ok();
    }
}