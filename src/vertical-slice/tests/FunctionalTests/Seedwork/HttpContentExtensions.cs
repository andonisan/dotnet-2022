using System.Text.Json;

namespace FunctionalTests.Seedwork
{
    internal static class HttpContentExtensions
    {
        private static JsonSerializerOptions jsonOptions = new JsonSerializerOptions()
        {
            PropertyNameCaseInsensitive = true,
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        };

        public static async Task<TData?> ReadAsJsonAsync<TData>(this HttpContent content, CancellationToken cancellationToken = default)
        {
            var stream = await content.ReadAsStreamAsync(cancellationToken);
            stream.Seek(0, SeekOrigin.Begin);

            return await JsonSerializer.DeserializeAsync<TData>(stream, jsonOptions, cancellationToken);
        }
    }
}
