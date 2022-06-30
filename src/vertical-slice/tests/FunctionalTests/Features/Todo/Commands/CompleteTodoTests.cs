using Microsoft.EntityFrameworkCore;
using TodoApp.App.Infrastructure.Persistence;
using TodoApp.App.IntegrationEventsModule.Persistence;
using Xunit.Abstractions;

namespace FunctionalTests.Features.Todo.Commands;

[Collection(nameof(SliceFixture))]
public class CompleteTodoTestsShould
{
    private readonly SliceFixture _fixture;

    public CompleteTodoTestsShould(SliceFixture serverFixture)
    {
        _fixture = serverFixture;
    }

    [Fact]
    [ResetDatabase]
    public async Task response_ok_when_complete_one_todo()
    {
        // Arrange
        var todo = await _fixture.CreateDefaultTodo();

        // Act
        var response = await _fixture.Client.PutAsync(ApiDefinition.V1.Todo.CompleteTodo(todo.Id), null);

        // Assert
        response.Should().NotBeNull();
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        await _fixture.ExecuteDbContextAsync<TodoDbContext>(async (context) =>
        {
            var dbTodo = await context.Todos.FirstOrDefaultAsync();
            dbTodo.Should().NotBeNull();
            dbTodo!.Completed.Should().Be(true);
        });
        
        await _fixture.ExecuteDbContextAsync<IntegrationEventContext>(async (context) =>
        {
            var integrationEventEntry = await context.IntegrationEvents.FirstOrDefaultAsync();
            integrationEventEntry.Should().NotBeNull();
            integrationEventEntry!.Content.Should().Contain(todo.Id);
        });
        
    }
}