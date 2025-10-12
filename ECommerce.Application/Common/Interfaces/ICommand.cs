namespace ECommerce.Application.Common.Interfaces;

/// <summary>
/// Command interface'i
/// </summary>
/// <typeparam name="TResponse">Response tipi</typeparam>
public interface ICommand<out TResponse>
{
}

/// <summary>
/// Command interface'i (Response olmadan)
/// </summary>
public interface ICommand
{
}