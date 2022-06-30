using System.Net.Http.Json;

namespace TodoApp.App.Services;

public interface IEffotCalculator
{
    Task<int> GetDaysEffort(Domain.Entities.Todo todo, Developer developer, CancellationToken cancellationToken);
}

public class EffotCalculator : IEffotCalculator
{
    private readonly HttpClient _httpClient;

    public EffotCalculator(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<int> GetDaysEffort(Domain.Entities.Todo todo, Developer developer, CancellationToken cancellationToken)
    {
        var response = await _httpClient
            .GetAsync(requestUri: $"/external-api/estimate-work?title={todo.Title}&developer={developer.Mail}",
                cancellationToken: cancellationToken);

        response.EnsureSuccessStatusCode();

        var daysEffort = await response.Content
            .ReadFromJsonAsync<int>(cancellationToken: cancellationToken);
        return daysEffort;
    }
}