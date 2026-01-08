namespace Deliris.BuildingBlocks.Application.Abstractions;

/// <summary>
/// Interface for queries that return a value.
/// Queries represent read operations that don't change system state.
/// </summary>
/// <typeparam name="TResponse">The type of the response.</typeparam>
public interface IQuery<out TResponse> : IRequest<TResponse>
{
}
