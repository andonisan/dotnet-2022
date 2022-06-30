using TodoApp.App.Domain.Entities;

namespace UnitTests.Domain;

public class TodoTests
{
    private const string Title = "test";

    [Fact]
    public void Creation()
    {
        var todo = new Todo(Title);
        todo.Title.Should().Be(Title);
        todo.Completed.Should().BeFalse();
        
        todo.MarkComplete();
        todo.Completed.Should().BeTrue();
    }
}
