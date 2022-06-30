using TodoApp.App.Domain.Entities;

namespace TodoApp.App.Repositories;

public interface ITodoRepository
{
    Task<Todo> Get(string id);
    Task<IEnumerable<Todo>> GetAll();
    Task Insert(Todo todo);
    Task Delete(Todo todo);
    Task Update(Todo todo);
}