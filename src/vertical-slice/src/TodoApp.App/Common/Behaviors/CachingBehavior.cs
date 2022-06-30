using ColoredConsole;
using EasyCaching.Core;

namespace TodoApp.App.Common.Behaviors;

public static class Cache
{
    public const string CacheDefaultName = "default";
}

public interface ICacheRequest
{
    string CacheKey { get; }
    DateTime? AbsoluteExpirationRelativeToNow { get; } 
}

public class CachingBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    where TRequest : notnull, IRequest<TResponse>
    where TResponse : notnull
{
    private readonly ILogger<CachingBehavior<TRequest, TResponse>> _logger;
    private readonly IEasyCachingProvider _cachingProvider;
    private readonly int defaultCacheExpirationInHours = 1;

    public CachingBehavior(IEasyCachingProviderFactory cachingFactory,
        ILogger<CachingBehavior<TRequest, TResponse>> logger)
    {
        _logger = logger;
        _cachingProvider = cachingFactory.GetCachingProvider(Cache.CacheDefaultName);
    }

    public async Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken,
        RequestHandlerDelegate<TResponse> next)
    {
        if (request is not IBaseQuery)
        {
            return await next();
        }

        if (request is not ICacheRequest cacheRequest)
        {
            return await next();
        }

        var cacheKey = cacheRequest.CacheKey;
        var cachedResponse = await _cachingProvider.GetAsync<TResponse>(cacheKey);
        if (cachedResponse.Value != null)
        {
            ColorConsole
                .WriteLine($"Fetch data from cache with cacheKey: {cacheKey}".Yellow());
            return cachedResponse.Value;
        }

        var response = await next();

        var expirationTime = cacheRequest.AbsoluteExpirationRelativeToNow ??
                             DateTime.Now.AddHours(defaultCacheExpirationInHours);

        // ReSharper disable once ConditionIsAlwaysTrueOrFalse
        if (response == null)
        {
            return response;
        }
        
        await _cachingProvider.SetAsync(cacheKey, response, expirationTime.TimeOfDay);

        ColorConsole.WriteLine($"Set data to cache with  cacheKey: {cacheKey}".Green());

        return response;
    }
}