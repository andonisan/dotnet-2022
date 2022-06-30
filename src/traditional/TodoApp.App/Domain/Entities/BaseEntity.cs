namespace TodoApp.App.Domain.Entities
{
    public class BaseEntity
    {
        public string Id { get; protected set; } = Guid.NewGuid().ToString();
    }
}
