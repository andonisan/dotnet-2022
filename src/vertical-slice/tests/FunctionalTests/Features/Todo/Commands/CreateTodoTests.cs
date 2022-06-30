using Microsoft.EntityFrameworkCore;
using TodoApp.App.Features.Todo.Commands;
using TodoApp.App.Infrastructure.Persistence;

namespace FunctionalTests.Features.Todo.Commands;

// [Collection(nameof(SliceFixture))]
// public class CreateTodoTestsShould
// {
//     private readonly SliceFixture _fixture;
//
//     public CreateTodoTestsShould(SliceFixture serverFixture)
//     {
//         _fixture = serverFixture;
//     }
//
//     [Fact]
//     [ResetDatabase]
//     public async Task response_ok_when_create_one_todo()
//     {
//         // Arrange
//         var command = new CreateTodo.Command("test");
//
//         // Act
//         var response = await _fixture.Client.PostAsync(ApiDefinition.V1.Todo.CreateTodo(), command);
//
//         // Assert
//         response.Should().NotBeNull();
//         response.StatusCode.Should().Be(HttpStatusCode.OK);
//
//         await _fixture.ExecuteDbContextAsync<TodoDbContext>(async (context) =>
//         {
//             var dbTodo = await context.Todos.FirstOrDefaultAsync();
//             dbTodo.Should().NotBeNull();
//             dbTodo!.Title.Should().Be(command.Title);
//             dbTodo.Completed.Should().Be(false);
//         });
//     }
//
//     [Fact]
//     public async Task response_bad_request_when_title_invalid()
//     {
//         // Arrange
//         var command = new CreateTodo.Command(string.Empty);
//
//         // Act
//         var response = await _fixture.Client.PostAsync(ApiDefinition.V1.Todo.CreateTodo(), command);
//
//         // Assert
//         response.Should().NotBeNull();
//         response.StatusCode.Should().Be(HttpStatusCode.Conflict);
//     }
// }