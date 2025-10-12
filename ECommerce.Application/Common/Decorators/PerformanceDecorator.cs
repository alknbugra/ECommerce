using ECommerce.Application.Common.Interfaces;
using Microsoft.Extensions.Logging;
using System.Diagnostics;

namespace ECommerce.Application.Common.Decorators;

/// <summary>
/// Performance decorator
/// </summary>
/// <typeparam name="TRequest">Request tipi</typeparam>
/// <typeparam name="TResponse">Response tipi</typeparam>
public class PerformanceDecorator<TRequest, TResponse> : HandlerDecoratorBase<TRequest, TResponse>
{
    private readonly ILogger<PerformanceDecorator<TRequest, TResponse>> _logger;
    private readonly long _slowOperationThresholdMs;

    public PerformanceDecorator(
        IHandler<TRequest, TResponse> handler,
        ILogger<PerformanceDecorator<TRequest, TResponse>> logger,
        long slowOperationThresholdMs = 1000) : base(handler)
    {
        _logger = logger;
        _slowOperationThresholdMs = slowOperationThresholdMs;
    }

    public override async Task<TResponse> HandleAsync(TRequest request, CancellationToken cancellationToken = default)
    {
        var requestType = typeof(TRequest).Name;
        var stopwatch = Stopwatch.StartNew();

        try
        {
            _logger.LogDebug("Starting {RequestType} request", requestType);
            
            var result = await _handler.HandleAsync(request, cancellationToken);
            
            stopwatch.Stop();
            var elapsedMs = stopwatch.ElapsedMilliseconds;

            if (elapsedMs > _slowOperationThresholdMs)
            {
                _logger.LogWarning("Slow operation detected: {RequestType} took {ElapsedMs}ms (threshold: {ThresholdMs}ms)", 
                    requestType, elapsedMs, _slowOperationThresholdMs);
            }
            else
            {
                _logger.LogDebug("Completed {RequestType} request in {ElapsedMs}ms", requestType, elapsedMs);
            }

            return result;
        }
        catch (Exception ex)
        {
            stopwatch.Stop();
            var elapsedMs = stopwatch.ElapsedMilliseconds;
            
            _logger.LogError(ex, "Failed {RequestType} request after {ElapsedMs}ms", requestType, elapsedMs);
            throw;
        }
    }
}

/// <summary>
/// Performance decorator (Request olmadan)
/// </summary>
/// <typeparam name="TResponse">Response tipi</typeparam>
public class PerformanceDecorator<TResponse> : HandlerDecoratorBase<TResponse>
{
    private readonly ILogger<PerformanceDecorator<TResponse>> _logger;
    private readonly long _slowOperationThresholdMs;

    public PerformanceDecorator(
        IHandler<TResponse> handler,
        ILogger<PerformanceDecorator<TResponse>> logger,
        long slowOperationThresholdMs = 1000) : base(handler)
    {
        _logger = logger;
        _slowOperationThresholdMs = slowOperationThresholdMs;
    }

    public override async Task<TResponse> HandleAsync(CancellationToken cancellationToken = default)
    {
        var responseType = typeof(TResponse).Name;
        var stopwatch = Stopwatch.StartNew();

        try
        {
            _logger.LogDebug("Starting request that returns {ResponseType}", responseType);
            
            var result = await _handler.HandleAsync(cancellationToken);
            
            stopwatch.Stop();
            var elapsedMs = stopwatch.ElapsedMilliseconds;

            if (elapsedMs > _slowOperationThresholdMs)
            {
                _logger.LogWarning("Slow operation detected: request that returns {ResponseType} took {ElapsedMs}ms (threshold: {ThresholdMs}ms)", 
                    responseType, elapsedMs, _slowOperationThresholdMs);
            }
            else
            {
                _logger.LogDebug("Completed request that returns {ResponseType} in {ElapsedMs}ms", responseType, elapsedMs);
            }

            return result;
        }
        catch (Exception ex)
        {
            stopwatch.Stop();
            var elapsedMs = stopwatch.ElapsedMilliseconds;
            
            _logger.LogError(ex, "Failed request that returns {ResponseType} after {ElapsedMs}ms", responseType, elapsedMs);
            throw;
        }
    }
}
