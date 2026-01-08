namespace Deliris.BuildingBlocks.Domain.Exceptions;

/// <summary>
/// Exception thrown when an entity is in an invalid state for the requested operation.
/// </summary>
public sealed class InvalidEntityStateException : Abstractions.Exceptions.DomainException
{
    /// <summary>
    /// Gets the type of the entity that is in an invalid state.
    /// </summary>
    public Type EntityType { get; }

    /// <summary>
    /// Gets the identifier of the entity that is in an invalid state.
    /// </summary>
    public object? EntityId { get; }

    /// <summary>
    /// Initializes a new instance of the <see cref="InvalidEntityStateException"/> class.
    /// </summary>
    /// <param name="entityType">The type of the entity that is in an invalid state.</param>
    /// <param name="entityId">The identifier of the entity that is in an invalid state.</param>
    /// <param name="message">The error message that explains the reason for the exception.</param>
    public InvalidEntityStateException(Type entityType, object? entityId, string message)
        : base(message, "INVALID_ENTITY_STATE")
    {
        EntityType = entityType;
        EntityId = entityId;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="InvalidEntityStateException"/> class.
    /// </summary>
    /// <param name="entityType">The type of the entity that is in an invalid state.</param>
    /// <param name="entityId">The identifier of the entity that is in an invalid state.</param>
    /// <param name="message">The error message that explains the reason for the exception.</param>
    /// <param name="innerException">The exception that is the cause of the current exception.</param>
    public InvalidEntityStateException(Type entityType, object? entityId, string message, Exception innerException)
        : base(message, "INVALID_ENTITY_STATE", innerException)
    {
        EntityType = entityType;
        EntityId = entityId;
    }
}
