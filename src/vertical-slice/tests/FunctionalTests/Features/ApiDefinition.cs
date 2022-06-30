namespace FunctionalTests.Features
{
    public static class ApiDefinition
    {
        public static class V1
        {
            public static class Todo
            {
                public static string GetTodos()
                {
                    return $"/todos";
                }
                
                public static string CreateTodo()
                {
                    return $"/todos";
                }
                
                public static string GetTodo(string id)
                {
                    return $"/todos/{id}";
                }
                
                public static string DeleteTodo(string id)
                {
                    return $"/todos/{id}";
                }

                public static string UpdateTodo(string id)
                {
                    return $"/todos/{id}";
                }
                
                public static string CompleteTodo(string id)
                {
                    return $"/todos/{id}/complete";
                }
            }
        }

    }
}
