namespace Deliris.BuildingBlocks.Domain.Abstractions.Entities;

/// <summary>
/// Base class for entities following DDD principles.
/// Entities are objects that have a distinct identity that runs through time and different states.
/// Two entities with the same identity are considered equal, regardless of their other properties.
/// </summary>
/// <typeparam name="TId">The type of the entity identifier. Must be a non-nullable type.</typeparam>
public abstract class Entity<TId> : IEntity<TId>, IEquatable<Entity<TId>> where TId : notnull
{
    /// <summary>
    /// Gets the unique identifier of the entity.
    /// </summary>
    public TId Id { get; protected init; } = default!;

    /// <summary>
    /// Initializes a new instance of the <see cref="Entity{TId}"/> class.
    /// </summary>
    /// <param name="id">The unique identifier of the entity.</param>
    /// <exception cref="ArgumentNullException">Thrown when id is null.</exception>
    protected Entity(TId id)
    {
        ArgumentNullException.ThrowIfNull(id);
        Id = id;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="Entity{TId}"/> class.
    /// Parameterless constructor for ORM support.
    /// </summary>
    protected Entity() { }

    /// <summary>
    /// Determines whether the specified object is equal to the current entity.
    /// Entities are compared by their identifiers.
    /// </summary>
    /// <param name="obj">The object to compare with the current entity.</param>
    /// <returns>true if the specified object is equal to the current entity; otherwise, false.</returns>
    public override bool Equals(object? obj)
    {
        if (obj is null) return false;
        if (ReferenceEquals(this, obj)) return true;
        if (obj.GetType() != GetType()) return false;
        return obj is Entity<TId> entity && Equals(entity);
    }

    /// <summary>
    /// Determines whether the specified entity is equal to the current entity.
    /// Entities are compared by their identifiers.
    /// </summary>
    /// <param name="other">The entity to compare with the current entity.</param>
    /// <returns>true if the specified entity is equal to the current entity; otherwise, false.</returns>
    public bool Equals(Entity<TId>? other)
    {
        if (other is null) return false;
        if (ReferenceEquals(this, other)) return true;
        if (other.GetType() != GetType()) return false;
        return EqualityComparer<TId>.Default.Equals(Id, other.Id);
    }

    /// <summary>
    /// Serves as the default hash function.
    /// </summary>
    /// <returns>A hash code for the current entity based on its identifier.</returns>
    public override int GetHashCode()
    {
        return EqualityComparer<TId>.Default.GetHashCode(Id);
    }

    /// <summary>
    /// Determines whether two entities are equal.
    /// </summary>
    /// <param name="left">The first entity to compare.</param>
    /// <param name="right">The second entity to compare.</param>
    /// <returns>true if the entities are equal; otherwise, false.</returns>
    public static bool operator ==(Entity<TId>? left, Entity<TId>? right)
    {
        if (left is null) return right is null;
        return left.Equals(right);
    }

    /// <summary>
    /// Determines whether two entities are not equal.
    /// </summary>
    /// <param name="left">The first entity to compare.</param>
    /// <param name="right">The second entity to compare.</param>
    /// <returns>true if the entities are not equal; otherwise, false.</returns>
    public static bool operator !=(Entity<TId>? left, Entity<TId>? right)
    {
        return !(left == right);
    }
}
