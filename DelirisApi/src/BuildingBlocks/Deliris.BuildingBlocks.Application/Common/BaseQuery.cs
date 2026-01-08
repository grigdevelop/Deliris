namespace Deliris.BuildingBlocks.Application.Common;

/// <summary>
/// Base class for queries.
/// </summary>
/// <typeparam name="TResponse">The type of the response.</typeparam>
public abstract record BaseQuery<TResponse> : IRequest<TResponse>
{
}
