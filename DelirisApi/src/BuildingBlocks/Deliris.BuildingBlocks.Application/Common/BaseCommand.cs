namespace Deliris.BuildingBlocks.Application.Common;

/// <summary>
/// Base class for commands that don't return a value.
/// </summary>
public abstract record BaseCommand : IRequest<Result>
{
}

/// <summary>
/// Base class for commands that return a value.
/// </summary>
/// <typeparam name="TResponse">The type of the response.</typeparam>
public abstract record BaseCommand<TResponse> : IRequest<TResponse>
{
}
