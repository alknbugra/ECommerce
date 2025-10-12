namespace ECommerce.Application.Common.Interfaces;

/// <summary>
/// Generic handler interface'i
/// </summary>
/// <typeparam name="TRequest">Request tipi</typeparam>
/// <typeparam name="TResponse">Response tipi</typeparam>
public interface IHandler<in TRequest, TResponse>
{
    /// <summary>
    /// Request'i işle
    /// </summary>
    /// <param name="request">Request</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Response</returns>
    Task<TResponse> HandleAsync(TRequest request, CancellationToken cancellationToken = default);
}

/// <summary>
/// Request olmayan handler interface'i
/// </summary>
/// <typeparam name="TResponse">Response tipi</typeparam>
public interface IHandler<TResponse>
{
    /// <summary>
    /// İşle
    /// </summary>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Response</returns>
    Task<TResponse> HandleAsync(CancellationToken cancellationToken = default);
}

/// <summary>
/// Base handler interface'i
/// </summary>
public interface IHandler
{
}
