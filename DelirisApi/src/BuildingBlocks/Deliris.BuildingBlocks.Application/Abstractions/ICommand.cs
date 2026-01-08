namespace Deliris.BuildingBlocks.Application.Abstractions;

/// <summary>
/// Marker interface for commands that don't return a value.
/// Commands represent write operations that change system state.
/// </summary>
public interface ICommand : IRequest<Result>
{
}

/// <summary>
/// Interface for commands that return a value.
/// Commands represent write operations that change system state.
/// </summary>
/// <typeparam name="TResponse">The type of the response.</typeparam>
public interface ICommand<out TResponse> : IRequest<TResponse>
{
}
