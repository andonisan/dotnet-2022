using TodoApp.App.Features.Todo.Queries;

namespace FunctionalTests.Features.Todo.Queries;

[Collection(nameof(SliceFixture))]
public class GetTodoTestsShould
{
    private readonly SliceFixture _fixture;

    public GetTodoTestsShould(SliceFixture serverFixture)
    {
        ArgumentNullException.ThrowIfNull(serverFixture);
        _fixture = serverFixture;
    }

    [Fact]
    [ResetDatabase]
    public async Task response_ok_when_id_is_valid_one()
    {
        // Arrange
        var todo = await _fixture.CreateDefaultTodo();

        // Act

        var response = await _fixture.Client.GetAsync(ApiDefinition.V1.Todo.GetTodo(todo.Id));

        // Assert
        response.StatusCode
            .Should().Be(HttpStatusCode.OK);

        var content = await response.Content.ReadAsJsonAsync<GetTodo.Response>();

        content.Should()
            .NotBeNull();

        content!.Id.Should().Be(todo.Id);
        content!.Caption.Should().Be(todo.Title);
        content!.IsCompleted.Should().Be(todo.Completed);
    }

    [Fact]
    public async Task response_not_found_when_id_is_not_existing_one()
    {
        // Arrange
        // Act
        var response = await _fixture.Client.GetAsync(ApiDefinition.V1.Todo.GetTodo("not_existing"));

        // Assert
        response.StatusCode
            .Should().Be(HttpStatusCode.NotFound);
    }
}