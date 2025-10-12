using ECommerce.Application.Common.Interfaces;
using Microsoft.Extensions.Logging;

namespace ECommerce.Application.Common.Decorators;

/// <summary>
/// Retry decorator
/// </summary>
/// <typeparam name="TRequest">Request tipi</typeparam>
/// <typeparam name="TResponse">Response tipi</typeparam>
public class RetryDecorator<TRequest, TResponse> : HandlerDecoratorBase<TRequest, TResponse>
{
    private readonly ILogger<RetryDecorator<TRequest, TResponse>> _logger;
    private readonly int _maxRetryAttempts;
    private readonly TimeSpan _delayBetweenRetries;

    public RetryDecorator(
        IHandler<TRequest, TResponse> handler,
        ILogger<RetryDecorator<TRequest, TResponse>> logger,
        int maxRetryAttempts = 3,
        TimeSpan? delayBetweenRetries = null) : base(handler)
    {
        _logger = logger;
        _maxRetryAttempts = maxRetryAttempts;
        _delayBetweenRetries = delayBetweenRetries ?? TimeSpan.FromMilliseconds(500);
    }

    public override async Task<TResponse> HandleAsync(TRequest request, CancellationToken cancellationToken = default)
    {
        var requestType = typeof(TRequest).Name;
        Exception? lastException = null;

        for (int attempt = 1; attempt <= _maxRetryAttempts; attempt++)
        {
            try
            {
                _logger.LogDebug("Attempting {RequestType} request (attempt {Attempt}/{MaxAttempts})", 
                    requestType, attempt, _maxRetryAttempts);

                var result = await _handler.HandleAsync(request, cancellationToken);
                
                if (attempt > 1)
                {
                    _logger.LogInformation("Successfully handled {RequestType} request on attempt {Attempt}", 
                        requestType, attempt);
                }

                return result;
            }
            catch (Exception ex) when (attempt < _maxRetryAttempts && ShouldRetry(ex))
            {
                lastException = ex;
                _logger.LogWarning(ex, "Attempt {Attempt} failed for {RequestType} request, retrying in {Delay}ms", 
                    attempt, requestType, _delayBetweenRetries.TotalMilliseconds);

                await Task.Delay(_delayBetweenRetries, cancellationToken);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Final attempt {Attempt} failed for {RequestType} request", 
                    attempt, requestType);
                throw;
            }
        }

        throw lastException ?? new InvalidOperationException($"Failed to handle {requestType} request after {_maxRetryAttempts} attempts");
    }

    private static bool ShouldRetry(Exception exception)
    {
        // Sadece geçici hatalar için retry yap
        return exception is TimeoutException ||
               exception is HttpRequestException ||
               exception is TaskCanceledException ||
               (exception is InvalidOperationException && exception.Message.Contains("timeout"));
    }
}

/// <summary>
/// Retry decorator (Request olmadan)
/// </summary>
/// <typeparam name="TResponse">Response tipi</typeparam>
public class RetryDecorator<TResponse> : HandlerDecoratorBase<TResponse>
{
    private readonly ILogger<RetryDecorator<TResponse>> _logger;
    private readonly int _maxRetryAttempts;
    private readonly TimeSpan _delayBetweenRetries;

    public RetryDecorator(
        IHandler<TResponse> handler,
        ILogger<RetryDecorator<TResponse>> logger,
        int maxRetryAttempts = 3,
        TimeSpan? delayBetweenRetries = null) : base(handler)
    {
        _logger = logger;
        _maxRetryAttempts = maxRetryAttempts;
        _delayBetweenRetries = delayBetweenRetries ?? TimeSpan.FromMilliseconds(500);
    }

    public override async Task<TResponse> HandleAsync(CancellationToken cancellationToken = default)
    {
        var responseType = typeof(TResponse).Name;
        Exception? lastException = null;

        for (int attempt = 1; attempt <= _maxRetryAttempts; attempt++)
        {
            try
            {
                _logger.LogDebug("Attempting request that returns {ResponseType} (attempt {Attempt}/{MaxAttempts})", 
                    responseType, attempt, _maxRetryAttempts);

                var result = await _handler.HandleAsync(cancellationToken);
                
                if (attempt > 1)
                {
                    _logger.LogInformation("Successfully handled request that returns {ResponseType} on attempt {Attempt}", 
                        responseType, attempt);
                }

                return result;
            }
            catch (Exception ex) when (attempt < _maxRetryAttempts && ShouldRetry(ex))
            {
                lastException = ex;
                _logger.LogWarning(ex, "Attempt {Attempt} failed for request that returns {ResponseType}, retrying in {Delay}ms", 
                    attempt, responseType, _delayBetweenRetries.TotalMilliseconds);

                await Task.Delay(_delayBetweenRetries, cancellationToken);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Final attempt {Attempt} failed for request that returns {ResponseType}", 
                    attempt, responseType);
                throw;
            }
        }

        throw lastException ?? new InvalidOperationException($"Failed to handle request that returns {responseType} after {_maxRetryAttempts} attempts");
    }

    private static bool ShouldRetry(Exception exception)
    {
        // Sadece geçici hatalar için retry yap
        return exception is TimeoutException ||
               exception is HttpRequestException ||
               exception is TaskCanceledException ||
               (exception is InvalidOperationException && exception.Message.Contains("timeout"));
    }
}
