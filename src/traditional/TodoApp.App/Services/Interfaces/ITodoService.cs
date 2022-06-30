using TodoApp.App.Dtos.Todos;

namespace TodoApp.App.Services.Interfaces;

public interface ITodoService
{
    Task<IEnumerable<TodoDto>> GetAllTodos();
    Task<TodoDto> GetTodo(string id);
    Task InsertTodo(CreateTodoDto createTodoDto);
    Task UpdateTodo(UpdateTodoDto updateTodo);
    Task DeleteTodo(string id);
    Task CompleteTodo(string id);
}