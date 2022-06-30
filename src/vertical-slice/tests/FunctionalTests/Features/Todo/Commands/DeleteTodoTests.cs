using Microsoft.EntityFrameworkCore;
using TodoApp.App.Infrastructure.Persistence;

namespace FunctionalTests.Features.Todo.Commands;

[Collection(nameof(SliceFixture))]
public class DeleteTodoTestsShould
{
    private readonly SliceFixture _fixture;

    public DeleteTodoTestsShould(SliceFixture serverFixture)
    {
        _fixture = serverFixture;
    }

    [Fact]
    [ResetDatabase]
    public async Task response_ok_when_delete_one_todo()
    {
        // Arrange
        var todo = await _fixture.CreateDefaultTodo();

        // Act
        var response = await _fixture.Client.DeleteAsync(ApiDefinition.V1.Todo.DeleteTodo(todo.Id));

        // Assert
        response.Should().NotBeNull();
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        await _fixture.ExecuteDbContextAsync<TodoDbContext>(async (context) =>
        {
            var dbTodo = await context.Todos.FirstOrDefaultAsync();
            dbTodo.Should().BeNull();
        });
    }
}