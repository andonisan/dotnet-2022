using Microsoft.EntityFrameworkCore;
using TodoApp.App.Features.Todo.Commands;
using TodoApp.App.Infrastructure.Persistence;

namespace FunctionalTests.Features.Todo.Commands;

[Collection(nameof(SliceFixture))]
public class UpdateTodoTestsShould
{
    private readonly SliceFixture _fixture;

    public UpdateTodoTestsShould(SliceFixture serverFixture)
    {
        _fixture = serverFixture;
    }

    [Fact]
    [ResetDatabase]
    public async Task response_ok_when_update_one_todo()
    {
        // Arrange
        var todo = await _fixture.CreateDefaultTodo();

        var command = new UpdateTodo.Command()
        {
            Title = "new title"
        };

        // Act
        var response = await  _fixture.Client.PutAsync(ApiDefinition.V1.Todo.UpdateTodo(todo.Id), command);

        // Assert
        response.Should().NotBeNull();
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        await _fixture.ExecuteDbContextAsync<TodoDbContext>(async (context) =>
        {
            var dbTodo = await context.Todos.FirstOrDefaultAsync();
            dbTodo.Should().NotBeNull();
            dbTodo!.Title.Should().Be(command.Title);
            dbTodo.Completed.Should().Be(false);
        });
    }
    
    [Fact]
    public async Task response_bad_request_when_title_invalid()
    {
        // Arrange
        var command = new UpdateTodo.Command()
        {
            Title = string.Empty
        };

        // Act
        var response = await _fixture.Client.PutAsync(ApiDefinition.V1.Todo.UpdateTodo("test"), command);

        // Assert
        response.Should().NotBeNull();
        response.StatusCode.Should().Be(HttpStatusCode.Conflict);
    }
}