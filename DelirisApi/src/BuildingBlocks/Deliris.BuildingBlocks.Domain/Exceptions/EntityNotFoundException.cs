namespace Deliris.BuildingBlocks.Domain.Exceptions;

/// <summary>
/// Exception thrown when an entity is not found.
/// </summary>
public sealed class EntityNotFoundException : Abstractions.Exceptions.DomainException
{
    /// <summary>
    /// Gets the type of the entity that was not found.
    /// </summary>
    public Type EntityType { get; }

    /// <summary>
    /// Gets the identifier of the entity that was not found.
    /// </summary>
    public object EntityId { get; }

    /// <summary>
    /// Initializes a new instance of the <see cref="EntityNotFoundException"/> class.
    /// </summary>
    /// <param name="entityType">The type of the entity that was not found.</param>
    /// <param name="entityId">The identifier of the entity that was not found.</param>
    public EntityNotFoundException(Type entityType, object entityId)
        : base($"Entity of type '{entityType.Name}' with identifier '{entityId}' was not found.", "ENTITY_NOT_FOUND")
    {
        EntityType = entityType;
        EntityId = entityId;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="EntityNotFoundException"/> class.
    /// </summary>
    /// <param name="entityType">The type of the entity that was not found.</param>
    /// <param name="entityId">The identifier of the entity that was not found.</param>
    /// <param name="innerException">The exception that is the cause of the current exception.</param>
    public EntityNotFoundException(Type entityType, object entityId, Exception innerException)
        : base($"Entity of type '{entityType.Name}' with identifier '{entityId}' was not found.", "ENTITY_NOT_FOUND", innerException)
    {
        EntityType = entityType;
        EntityId = entityId;
    }
}
