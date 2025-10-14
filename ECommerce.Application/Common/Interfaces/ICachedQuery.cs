using ECommerce.Application.Common.Messaging;

namespace ECommerce.Application.Common.Interfaces;

/// <summary>
/// Represents a query that can be cached to improve performance.
/// </summary>
/// <typeparam name="TResponse">The type of response returned by the query.</typeparam>
public interface ICachedQuery<TResponse> : IQuery<TResponse>
{
    /// <summary>
    /// Gets the cache key for this query.
    /// </summary>
    string CacheKey { get; }

    /// <summary>
    /// Gets the cache expiration time for this query.
    /// </summary>
    TimeSpan Expiration { get; }
}
