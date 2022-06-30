using TodoApp.App.Domain.Entities;
using TodoApp.App.Dtos.Todos;
using TodoApp.App.Repositories;
using TodoApp.App.Services.Interfaces;

namespace TodoApp.App.Services;

public class TodoService : ITodoService
{
    private readonly ITodoRepository _repository;

    public TodoService(ITodoRepository repository)
    {
        _repository = repository;
    }
    
    public async Task<IEnumerable<TodoDto>> GetAllTodos()
    {
        var todos = await _repository.GetAll();
        return todos
            .Select(t => new TodoDto(t.Id, t.Title, t.Completed))
            .ToList();
    }

    public async Task<TodoDto> GetTodo(string id)
    {
        var todo =  await _repository.Get(id);
        return new TodoDto(todo.Id, todo.Title, todo.Completed);
    }

    public async Task InsertTodo(CreateTodoDto todoDto)
    {
        var todo = new Todo(todoDto.Title);
        await _repository.Insert(todo);
    }

    public async Task UpdateTodo(UpdateTodoDto updateTodo)
    {
        var todo = await _repository.Get(updateTodo.TodoId);
        todo.UpdateTitle(updateTodo.Title);
        await _repository.Update(todo);
    }
    
    public async Task DeleteTodo(string id)
    {
        var todo = await _repository.Get(id);
        await _repository.Delete(todo);
    }
    
    public async Task CompleteTodo(string id)
    {
        var todo = await _repository.Get(id);
        todo.MarkComplete();
        await _repository.Update(todo);
    }
}