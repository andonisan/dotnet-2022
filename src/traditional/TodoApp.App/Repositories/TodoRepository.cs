using Microsoft.EntityFrameworkCore;
using TodoApp.App.Domain.Entities;
using TodoApp.App.Persistence;

namespace TodoApp.App.Repositories;

public class TodoRepository : ITodoRepository
{
    private readonly TodoDbContext _context;

    public TodoRepository(TodoDbContext context)
    {
        _context = context;
    }

    public async Task<Todo> Get(string id)
    {
        return await _context.Todos
            .SingleOrDefaultAsync(c => c.Id == id);
    }

    public async Task<IEnumerable<Todo>> GetAll()
    {
        return await _context.Todos
            .ToArrayAsync();
    }

    public async Task Insert(Todo todo)
    {
        ArgumentNullException.ThrowIfNull(todo);

        await _context.AddAsync(todo);
        await _context.SaveChangesAsync();
    }

    public async Task Update(Todo todo)
    {
        ArgumentNullException.ThrowIfNull(todo);

        _context.Update(todo);
        await _context.SaveChangesAsync();
    }

    public async Task Delete(Todo todo)
    {
        ArgumentNullException.ThrowIfNull(todo);

        _context.Remove(todo);
        await _context.SaveChangesAsync();
    }
}