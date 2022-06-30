using System.Text;
using System.Text.Json;

namespace FunctionalTests.Seedwork;

internal static class HttpClientExtensions
{
    internal static async Task<HttpResponseMessage> PostAsync(this HttpClient httpClient, string requestUri,
        object content)
    {
        return await httpClient.PostAsync(requestUri, CreateHttpContent(content));
    }

    internal static async Task<HttpResponseMessage> PutAsync(this HttpClient httpClient, string requestUri,
        object content)
    {
        return await httpClient.PutAsync(requestUri, CreateHttpContent(content));
    }

    private static HttpContent CreateHttpContent(object data)
    {
        var content = JsonSerializer.Serialize(data);
        return new StringContent(content, Encoding.Unicode, "application/json");
    }
}