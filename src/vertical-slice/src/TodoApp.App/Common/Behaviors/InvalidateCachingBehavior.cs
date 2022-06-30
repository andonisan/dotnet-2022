using ColoredConsole;
using EasyCaching.Core;

namespace TodoApp.App.Common.Behaviors;

internal interface IInvalidateCacheRequest
{
   public string PrefixCacheKey { get; }
}

public class InvalidateCachingBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    where TRequest : notnull, IRequest<TResponse>
    where TResponse : notnull
{
    private readonly ILogger<InvalidateCachingBehavior<TRequest, TResponse>> _logger;
    private readonly IEasyCachingProvider _cachingProvider;

    public InvalidateCachingBehavior(IEasyCachingProviderFactory cachingFactory,
        ILogger<InvalidateCachingBehavior<TRequest, TResponse>> logger)
    {
        _logger = logger;
        _cachingProvider = cachingFactory.GetCachingProvider(Cache.CacheDefaultName);
    }

    public async Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken,
        RequestHandlerDelegate<TResponse> next)
    {
        if (request is not IBaseCommand)
        {
            return await next();
        }
        
        if (request is not IInvalidateCacheRequest cacheRequest)
        {
            return await next();
        }

        var cacheKey = cacheRequest.PrefixCacheKey;
        var response = await next();

        await _cachingProvider.RemoveByPrefixAsync(cacheKey);

        ColorConsole.WriteLine($"Cache data with cacheKey: {cacheKey} removed.".DarkRed());

        return response;
    }
}