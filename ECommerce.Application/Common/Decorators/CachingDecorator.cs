using ECommerce.Application.Common.Interfaces;
using ECommerce.Application.Common.Messaging;
using ECommerce.Application.Common.Results;
using Microsoft.Extensions.Logging;

namespace ECommerce.Application.Common.Decorators;

/// <summary>
/// Provides a caching decorator for query handlers, enabling caching of query results to improve performance and reduce
/// redundant processing.
/// </summary>
/// <remarks>This static class contains a nested <see cref="QueryHandler{TQuery, TResponse}"/> implementation that
/// wraps an inner query handler with caching functionality. The decorator checks the cache for a result before
/// delegating to the inner handler, and stores successful results in the cache for future use.</remarks>
internal static class CachingDecorator
{
    internal sealed class QueryHandler<TQuery, TResponse>(
        IQueryHandler<TQuery, TResponse> inner,
        ICacheService cacheService,
        ILogger<QueryHandler<TQuery, TResponse>> logger)
        : IQueryHandler<TQuery, TResponse>
        where TQuery : ICachedQuery<TResponse>
    {
        public async Task<Result<TResponse>> Handle(TQuery query, CancellationToken cancellationToken)
        {
            var name = typeof(TQuery).Name;

            var cachedResult = await cacheService.GetAsync<Result<TResponse>>(query.CacheKey, cancellationToken);

            if (cachedResult is not null)
            {
                logger.LogInformation("Cache hit for {Query}", name);
                return cachedResult;
            }

            logger.LogInformation("Cache miss for {Query}", name);

            var result = await inner.Handle(query, cancellationToken);

            if (result.IsSuccess)
            {
                await cacheService.SetAsync(query.CacheKey, result, query.Expiration, cancellationToken);
            }

            return result;
        }
    }
}
