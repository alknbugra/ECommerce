using ECommerce.Application.Common.Results;
using ECommerce.Application.Common.Messaging;
using Microsoft.Extensions.Logging;
using System.Diagnostics;

namespace ECommerce.Application.Common.Behaviors;

/// <summary>
/// Provides performance monitoring decorators for command and query handlers, enabling performance tracking and monitoring.
/// </summary>
public static class PerformanceDecorator
{
    public sealed class CommandBaseHandler<TCommand>(
        ICommandHandler<TCommand> inner,
        ILogger<CommandBaseHandler<TCommand>> logger)
        : ICommandHandler<TCommand> where TCommand : ICommand
    {
        public async Task<Result> Handle(TCommand command, CancellationToken cancellationToken)
        {
            var stopwatch = Stopwatch.StartNew();
            var commandName = typeof(TCommand).Name;
            
            try
            {
                logger.LogInformation("Performance: Starting command {CommandName}", commandName);
                
                var result = await inner.Handle(command, cancellationToken);
                
                stopwatch.Stop();
                logger.LogInformation("Performance: Command {CommandName} completed in {ElapsedMs}ms", 
                    commandName, stopwatch.ElapsedMilliseconds);
                
                return result;
            }
            catch (Exception ex)
            {
                stopwatch.Stop();
                logger.LogError(ex, "Performance: Command {CommandName} failed after {ElapsedMs}ms", 
                    commandName, stopwatch.ElapsedMilliseconds);
                throw;
            }
        }
    }

    public sealed class CommandHandler<TCommand, TResponse>(
        ICommandHandler<TCommand, TResponse> inner,
        ILogger<CommandHandler<TCommand, TResponse>> logger)
        : ICommandHandler<TCommand, TResponse> where TCommand : ICommand<TResponse>
    {
        public async Task<Result<TResponse>> Handle(TCommand command, CancellationToken cancellationToken)
        {
            var stopwatch = Stopwatch.StartNew();
            var commandName = typeof(TCommand).Name;
            
            try
            {
                logger.LogInformation("Performance: Starting command {CommandName}", commandName);
                
                var result = await inner.Handle(command, cancellationToken);
                
                stopwatch.Stop();
                logger.LogInformation("Performance: Command {CommandName} completed in {ElapsedMs}ms", 
                    commandName, stopwatch.ElapsedMilliseconds);
                
                return result;
            }
            catch (Exception ex)
            {
                stopwatch.Stop();
                logger.LogError(ex, "Performance: Command {CommandName} failed after {ElapsedMs}ms", 
                    commandName, stopwatch.ElapsedMilliseconds);
                throw;
            }
        }
    }

    public sealed class QueryHandler<TQuery, TResponse>(
        IQueryHandler<TQuery, TResponse> inner,
        ILogger<QueryHandler<TQuery, TResponse>> logger)
        : IQueryHandler<TQuery, TResponse> where TQuery : IQuery<TResponse>
    {
        public async Task<Result<TResponse>> Handle(TQuery query, CancellationToken cancellationToken)
        {
            var stopwatch = Stopwatch.StartNew();
            var queryName = typeof(TQuery).Name;
            
            try
            {
                logger.LogInformation("Performance: Starting query {QueryName}", queryName);
                
                var result = await inner.Handle(query, cancellationToken);
                
                stopwatch.Stop();
                logger.LogInformation("Performance: Query {QueryName} completed in {ElapsedMs}ms", 
                    queryName, stopwatch.ElapsedMilliseconds);
                
                return result;
            }
            catch (Exception ex)
            {
                stopwatch.Stop();
                logger.LogError(ex, "Performance: Query {QueryName} failed after {ElapsedMs}ms", 
                    queryName, stopwatch.ElapsedMilliseconds);
                throw;
            }
        }
    }
}
