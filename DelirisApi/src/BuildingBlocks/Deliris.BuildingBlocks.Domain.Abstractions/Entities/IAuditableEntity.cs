namespace Deliris.BuildingBlocks.Domain.Abstractions.Entities;

/// <summary>
/// Interface for entities that support audit tracking.
/// Provides properties to track creation and modification metadata.
/// </summary>
public interface IAuditableEntity
{
    /// <summary>
    /// Gets the date and time when the entity was created in UTC.
    /// </summary>
    DateTime CreatedAtUtc { get; }

    /// <summary>
    /// Gets the identifier of the user who created the entity.
    /// </summary>
    string? CreatedBy { get; }

    /// <summary>
    /// Gets the date and time when the entity was last updated in UTC.
    /// </summary>
    DateTime? UpdatedAtUtc { get; }

    /// <summary>
    /// Gets the identifier of the user who last updated the entity.
    /// </summary>
    string? UpdatedBy { get; }
}
