using ECommerce.Application.Common.Interfaces;
using ECommerce.Application.Common.Exceptions;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;

namespace ECommerce.Application.Common.Decorators;

/// <summary>
/// Validation decorator
/// </summary>
/// <typeparam name="TRequest">Request tipi</typeparam>
/// <typeparam name="TResponse">Response tipi</typeparam>
public class ValidationDecorator<TRequest, TResponse> : HandlerDecoratorBase<TRequest, TResponse>
{
    private readonly IServiceProvider _serviceProvider;

    public ValidationDecorator(
        IHandler<TRequest, TResponse> handler,
        IServiceProvider serviceProvider) : base(handler)
    {
        _serviceProvider = serviceProvider;
    }

    public override async Task<TResponse> HandleAsync(TRequest request, CancellationToken cancellationToken = default)
    {
        // Validator'ı bul
        var validator = _serviceProvider.GetService<IValidator<TRequest>>();
        
        if (validator != null)
        {
            var validationResult = await validator.ValidateAsync(request, cancellationToken);
            
            if (!validationResult.IsValid)
            {
                throw new Common.Exceptions.ValidationException(validationResult.Errors);
            }
        }

        return await _handler.HandleAsync(request, cancellationToken);
    }
}
