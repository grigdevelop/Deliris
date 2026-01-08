namespace Deliris.BuildingBlocks.Domain.Abstractions.Entities;

/// <summary>
/// Interface for entities that support soft deletion.
/// Soft deletion marks entities as deleted without physically removing them from the data store.
/// </summary>
public interface ISoftDeletable
{
    /// <summary>
    /// Gets a value indicating whether the entity is deleted.
    /// </summary>
    bool IsDeleted { get; }

    /// <summary>
    /// Gets the date and time when the entity was deleted in UTC.
    /// </summary>
    DateTime? DeletedAtUtc { get; }

    /// <summary>
    /// Gets the identifier of the user who deleted the entity.
    /// </summary>
    string? DeletedBy { get; }

    /// <summary>
    /// Marks the entity as deleted.
    /// </summary>
    /// <param name="deletedBy">The identifier of the user who deleted the entity.</param>
    /// <param name="deletedAtUtc">The date and time when the entity was deleted in UTC. If null, uses current UTC time.</param>
    void Delete(string? deletedBy, DateTime? deletedAtUtc = null);

    /// <summary>
    /// Restores a soft-deleted entity.
    /// </summary>
    void Restore();
}
