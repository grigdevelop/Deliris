namespace Deliris.BuildingBlocks.Domain.Abstractions.Entities;

/// <summary>
/// Base interface for entities with identity.
/// Entities are objects that have a distinct identity that runs through time and different states.
/// </summary>
/// <typeparam name="TId">The type of the entity identifier. Must be a non-nullable type.</typeparam>
public interface IEntity<TId> where TId : notnull
{
    /// <summary>
    /// Gets the unique identifier of the entity.
    /// </summary>
    TId Id { get; }
}
