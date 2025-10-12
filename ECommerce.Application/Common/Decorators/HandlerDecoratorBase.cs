using ECommerce.Application.Common.Interfaces;

namespace ECommerce.Application.Common.Decorators;

/// <summary>
/// Handler decorator base sınıfı
/// </summary>
/// <typeparam name="TRequest">Request tipi</typeparam>
/// <typeparam name="TResponse">Response tipi</typeparam>
public abstract class HandlerDecoratorBase<TRequest, TResponse> : IHandler<TRequest, TResponse>
{
    protected readonly IHandler<TRequest, TResponse> _handler;

    protected HandlerDecoratorBase(IHandler<TRequest, TResponse> handler)
    {
        _handler = handler;
    }

    public virtual async Task<TResponse> HandleAsync(TRequest request, CancellationToken cancellationToken = default)
    {
        return await _handler.HandleAsync(request, cancellationToken);
    }
}

/// <summary>
/// Handler decorator base sınıfı (Request olmadan)
/// </summary>
/// <typeparam name="TResponse">Response tipi</typeparam>
public abstract class HandlerDecoratorBase<TResponse> : IHandler<TResponse>
{
    protected readonly IHandler<TResponse> _handler;

    protected HandlerDecoratorBase(IHandler<TResponse> handler)
    {
        _handler = handler;
    }

    public virtual async Task<TResponse> HandleAsync(CancellationToken cancellationToken = default)
    {
        return await _handler.HandleAsync(cancellationToken);
    }
}
