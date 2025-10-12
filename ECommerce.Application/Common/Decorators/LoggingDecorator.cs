using ECommerce.Application.Common.Interfaces;
using Microsoft.Extensions.Logging;

namespace ECommerce.Application.Common.Decorators;

/// <summary>
/// Logging decorator
/// </summary>
/// <typeparam name="TRequest">Request tipi</typeparam>
/// <typeparam name="TResponse">Response tipi</typeparam>
public class LoggingDecorator<TRequest, TResponse> : HandlerDecoratorBase<TRequest, TResponse>
{
    private readonly ILogger<LoggingDecorator<TRequest, TResponse>> _logger;

    public LoggingDecorator(
        IHandler<TRequest, TResponse> handler,
        ILogger<LoggingDecorator<TRequest, TResponse>> logger) : base(handler)
    {
        _logger = logger;
    }

    public override async Task<TResponse> HandleAsync(TRequest request, CancellationToken cancellationToken = default)
    {
        var requestType = typeof(TRequest).Name;
        var responseType = typeof(TResponse).Name;

        _logger.LogInformation("Handling {RequestType} request", requestType);

        try
        {
            var response = await _handler.HandleAsync(request, cancellationToken);
            _logger.LogInformation("Successfully handled {RequestType} request, returning {ResponseType}", 
                requestType, responseType);
            return response;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error handling {RequestType} request", requestType);
            throw;
        }
    }
}

/// <summary>
/// Logging decorator (Request olmadan)
/// </summary>
/// <typeparam name="TResponse">Response tipi</typeparam>
public class LoggingDecorator<TResponse> : HandlerDecoratorBase<TResponse>
{
    private readonly ILogger<LoggingDecorator<TResponse>> _logger;

    public LoggingDecorator(
        IHandler<TResponse> handler,
        ILogger<LoggingDecorator<TResponse>> logger) : base(handler)
    {
        _logger = logger;
    }

    public override async Task<TResponse> HandleAsync(CancellationToken cancellationToken = default)
    {
        var responseType = typeof(TResponse).Name;

        _logger.LogInformation("Handling request that returns {ResponseType}", responseType);

        try
        {
            var response = await _handler.HandleAsync(cancellationToken);
            _logger.LogInformation("Successfully handled request, returning {ResponseType}", responseType);
            return response;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error handling request that returns {ResponseType}", responseType);
            throw;
        }
    }
}
