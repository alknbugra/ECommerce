namespace ECommerce.Application.Common.Interfaces;

/// <summary>
/// Query handler interface'i
/// </summary>
/// <typeparam name="TQuery">Query tipi</typeparam>
/// <typeparam name="TResponse">Response tipi</typeparam>
public interface IQueryHandler<in TQuery, TResponse> : IHandler<TQuery, TResponse>
{
}