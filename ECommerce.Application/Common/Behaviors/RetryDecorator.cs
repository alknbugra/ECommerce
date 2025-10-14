using ECommerce.Application.Common.Results;
using ECommerce.Application.Common.Messaging;
using Microsoft.Extensions.Logging;

namespace ECommerce.Application.Common.Behaviors;

/// <summary>
/// Provides retry decorators for command and query handlers, enabling automatic retry on transient failures.
/// </summary>
public static class RetryDecorator
{
    private const int MaxRetryAttempts = 3;
    private static readonly TimeSpan[] RetryDelays = 
    {
        TimeSpan.FromMilliseconds(100),
        TimeSpan.FromMilliseconds(500),
        TimeSpan.FromSeconds(1)
    };

    public sealed class CommandBaseHandler<TCommand>(
        ICommandHandler<TCommand> inner,
        ILogger<CommandBaseHandler<TCommand>> logger)
        : ICommandHandler<TCommand> where TCommand : ICommand
    {
        public async Task<Result> Handle(TCommand command, CancellationToken cancellationToken)
        {
            var commandName = typeof(TCommand).Name;
            
            for (int attempt = 0; attempt <= MaxRetryAttempts; attempt++)
            {
                try
                {
                    var result = await inner.Handle(command, cancellationToken);
                    
                    if (attempt > 0)
                    {
                        logger.LogInformation("Retry: Command {CommandName} succeeded on attempt {Attempt}", 
                            commandName, attempt + 1);
                    }
                    
                    return result;
                }
                catch (Exception ex) when (attempt < MaxRetryAttempts && IsTransientException(ex))
                {
                    var delay = RetryDelays[Math.Min(attempt, RetryDelays.Length - 1)];
                    logger.LogWarning(ex, "Retry: Command {CommandName} failed on attempt {Attempt}, retrying in {Delay}ms", 
                        commandName, attempt + 1, delay.TotalMilliseconds);
                    
                    await Task.Delay(delay, cancellationToken);
                }
                catch (Exception ex)
                {
                    logger.LogError(ex, "Retry: Command {CommandName} failed after {Attempts} attempts", 
                        commandName, MaxRetryAttempts + 1);
                    throw;
                }
            }
            
            return Result.Failure(Error.Problem("Command.RetryExhausted", "Command failed after maximum retry attempts"));
        }
    }

    public sealed class CommandHandler<TCommand, TResponse>(
        ICommandHandler<TCommand, TResponse> inner,
        ILogger<CommandHandler<TCommand, TResponse>> logger)
        : ICommandHandler<TCommand, TResponse> where TCommand : ICommand<TResponse>
    {
        public async Task<Result<TResponse>> Handle(TCommand command, CancellationToken cancellationToken)
        {
            var commandName = typeof(TCommand).Name;
            
            for (int attempt = 0; attempt <= MaxRetryAttempts; attempt++)
            {
                try
                {
                    var result = await inner.Handle(command, cancellationToken);
                    
                    if (attempt > 0)
                    {
                        logger.LogInformation("Retry: Command {CommandName} succeeded on attempt {Attempt}", 
                            commandName, attempt + 1);
                    }
                    
                    return result;
                }
                catch (Exception ex) when (attempt < MaxRetryAttempts && IsTransientException(ex))
                {
                    var delay = RetryDelays[Math.Min(attempt, RetryDelays.Length - 1)];
                    logger.LogWarning(ex, "Retry: Command {CommandName} failed on attempt {Attempt}, retrying in {Delay}ms", 
                        commandName, attempt + 1, delay.TotalMilliseconds);
                    
                    await Task.Delay(delay, cancellationToken);
                }
                catch (Exception ex)
                {
                    logger.LogError(ex, "Retry: Command {CommandName} failed after {Attempts} attempts", 
                        commandName, MaxRetryAttempts + 1);
                    throw;
                }
            }
            
            return Result.Failure<TResponse>(Error.Problem("Command.RetryExhausted", "Command failed after maximum retry attempts"));
        }
    }

    public sealed class QueryHandler<TQuery, TResponse>(
        IQueryHandler<TQuery, TResponse> inner,
        ILogger<QueryHandler<TQuery, TResponse>> logger)
        : IQueryHandler<TQuery, TResponse> where TQuery : IQuery<TResponse>
    {
        public async Task<Result<TResponse>> Handle(TQuery query, CancellationToken cancellationToken)
        {
            var queryName = typeof(TQuery).Name;
            
            for (int attempt = 0; attempt <= MaxRetryAttempts; attempt++)
            {
                try
                {
                    var result = await inner.Handle(query, cancellationToken);
                    
                    if (attempt > 0)
                    {
                        logger.LogInformation("Retry: Query {QueryName} succeeded on attempt {Attempt}", 
                            queryName, attempt + 1);
                    }
                    
                    return result;
                }
                catch (Exception ex) when (attempt < MaxRetryAttempts && IsTransientException(ex))
                {
                    var delay = RetryDelays[Math.Min(attempt, RetryDelays.Length - 1)];
                    logger.LogWarning(ex, "Retry: Query {QueryName} failed on attempt {Attempt}, retrying in {Delay}ms", 
                        queryName, attempt + 1, delay.TotalMilliseconds);
                    
                    await Task.Delay(delay, cancellationToken);
                }
                catch (Exception ex)
                {
                    logger.LogError(ex, "Retry: Query {QueryName} failed after {Attempts} attempts", 
                        queryName, MaxRetryAttempts + 1);
                    throw;
                }
            }
            
            return Result.Failure<TResponse>(Error.Problem("Query.RetryExhausted", "Query failed after maximum retry attempts"));
        }
    }

    /// <summary>
    /// Determines if an exception is transient and should be retried
    /// </summary>
    /// <param name="exception">The exception to check</param>
    /// <returns>True if the exception is transient and should be retried</returns>
    private static bool IsTransientException(Exception exception)
    {
        return exception switch
        {
            TimeoutException => true,
            HttpRequestException => true,
            TaskCanceledException => true,
            InvalidOperationException ex when ex.Message.Contains("timeout") => true,
            InvalidOperationException ex when ex.Message.Contains("connection") => true,
            _ => false
        };
    }
}
