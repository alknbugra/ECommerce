using ECommerce.Application.Common.Results;
using ECommerce.Application.Common.Messaging;
using Microsoft.Extensions.Logging;

namespace ECommerce.Application.Common.Behaviors;

/// <summary>
/// Provides logging decorators for command and query handlers, enabling logging of processing and completion events for
/// commands and queries.
/// </summary>
public static class LoggingDecorator
{
    public sealed class CommandBaseHandler<TCommand>(
        ICommandHandler<TCommand> inner,
        ILogger<CommandBaseHandler<TCommand>> logger)
        : ICommandHandler<TCommand> where TCommand : ICommand
    {
        public async Task<Result> Handle(TCommand command, CancellationToken cancellationToken)
        {
            var commandName = typeof(TCommand).Name;
            var commandId = Guid.NewGuid();
            
            logger.LogInformation("Processing command {CommandName} with ID {CommandId}", commandName, commandId);

            var stopwatch = System.Diagnostics.Stopwatch.StartNew();
            Result result = await inner.Handle(command, cancellationToken);
            stopwatch.Stop();

            if (result.IsSuccess)
            {
                logger.LogInformation("Successfully completed command {CommandName} with ID {CommandId} in {ElapsedMs}ms", 
                    commandName, commandId, stopwatch.ElapsedMilliseconds);
            }
            else
            {
                logger.LogError("Failed to complete command {CommandName} with ID {CommandId} in {ElapsedMs}ms. Error: {Error}", 
                    commandName, commandId, stopwatch.ElapsedMilliseconds, result.Error?.Description);
            }

            return result;
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
            var commandId = Guid.NewGuid();
            
            logger.LogInformation("Processing command {CommandName} with ID {CommandId}", commandName, commandId);

            var stopwatch = System.Diagnostics.Stopwatch.StartNew();
            Result<TResponse> result = await inner.Handle(command, cancellationToken);
            stopwatch.Stop();

            if (result.IsSuccess)
            {
                logger.LogInformation("Successfully completed command {CommandName} with ID {CommandId} in {ElapsedMs}ms", 
                    commandName, commandId, stopwatch.ElapsedMilliseconds);
            }
            else
            {
                logger.LogError("Failed to complete command {CommandName} with ID {CommandId} in {ElapsedMs}ms. Error: {Error}", 
                    commandName, commandId, stopwatch.ElapsedMilliseconds, result.Error?.Description);
            }

            return result;
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
            var queryId = Guid.NewGuid();
            
            logger.LogInformation("Processing query {QueryName} with ID {QueryId}", queryName, queryId);

            var stopwatch = System.Diagnostics.Stopwatch.StartNew();
            Result<TResponse> result = await inner.Handle(query, cancellationToken);
            stopwatch.Stop();

            if (result.IsSuccess)
            {
                logger.LogInformation("Successfully completed query {QueryName} with ID {QueryId} in {ElapsedMs}ms", 
                    queryName, queryId, stopwatch.ElapsedMilliseconds);
            }
            else
            {
                logger.LogError("Failed to complete query {QueryName} with ID {QueryId} in {ElapsedMs}ms. Error: {Error}", 
                    queryName, queryId, stopwatch.ElapsedMilliseconds, result.Error?.Description);
            }

            return result;
        }
    }
}
