namespace ECommerce.Application.Common.Interfaces;

/// <summary>
/// Command handler interface'i
/// </summary>
/// <typeparam name="TCommand">Command tipi</typeparam>
/// <typeparam name="TResponse">Response tipi</typeparam>
public interface ICommandHandler<in TCommand, TResponse> : IHandler<TCommand, TResponse>
{
}

/// <summary>
/// Command handler interface'i (Response olmadan)
/// </summary>
/// <typeparam name="TCommand">Command tipi</typeparam>
public interface ICommandHandler<in TCommand> : IHandler
{
    /// <summary>
    /// Command'i işle
    /// </summary>
    /// <param name="command">Command</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Task</returns>
    Task HandleAsync(TCommand command, CancellationToken cancellationToken = default);
}