using FunctionalTests.Seedwork.Builders;
using TodoApp.App.Domain.Entities;
using TodoApp.App.Infrastructure.Persistence;

namespace FunctionalTests.Seedwork
{
    internal static class SliceFixtureExtensions
    {
        
        public static async Task<Todo> CreateDefaultTodo(this SliceFixture serverFixture, CancellationToken cancellationToken = default)
        {
            var todo = TestBuilders.GetTodosBuilder()
                .Build();

            await serverFixture.InsertAsync<Todo, TodoDbContext>(todo);

            return todo;
        }
    }
}
