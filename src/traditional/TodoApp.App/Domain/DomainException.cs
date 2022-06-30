namespace TodoApp.App.Domain;

    public class DomainException : Exception
    {
        public string Title { get; init; }

        public string Description { get; init; }

        public DomainException(string title, string description)
            : base(description)
        {
            this.Title = title;
            this.Description = description;
        }
    }
