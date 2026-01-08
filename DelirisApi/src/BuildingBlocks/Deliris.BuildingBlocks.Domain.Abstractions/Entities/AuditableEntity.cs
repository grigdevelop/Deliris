namespace Deliris.BuildingBlocks.Domain.Abstractions.Entities;

/// <summary>
/// Base class for auditable entities that track creation and modification metadata.
/// </summary>
/// <typeparam name="TId">The type of the entity identifier.</typeparam>
public abstract class AuditableEntity<TId> : Entity<TId>, IAuditableEntity where TId : notnull
{
    /// <summary>
    /// Gets the date and time when the entity was created in UTC.
    /// </summary>
    public DateTime CreatedAtUtc { get; private set; }

    /// <summary>
    /// Gets the identifier of the user who created the entity.
    /// </summary>
    public string? CreatedBy { get; private set; }

    /// <summary>
    /// Gets the date and time when the entity was last updated in UTC.
    /// </summary>
    public DateTime? UpdatedAtUtc { get; private set; }

    /// <summary>
    /// Gets the identifier of the user who last updated the entity.
    /// </summary>
    public string? UpdatedBy { get; private set; }

    /// <summary>
    /// Initializes a new instance of the <see cref="AuditableEntity{TId}"/> class.
    /// </summary>
    /// <param name="id">The unique identifier of the entity.</param>
    protected AuditableEntity(TId id) : base(id)
    {
        CreatedAtUtc = DateTime.UtcNow;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="AuditableEntity{TId}"/> class.
    /// Parameterless constructor for ORM support.
    /// </summary>
    protected AuditableEntity() : base()
    {
    }

    /// <summary>
    /// Sets the creation audit information.
    /// </summary>
    /// <param name="createdBy">The identifier of the user who created the entity.</param>
    /// <param name="createdAtUtc">The date and time when the entity was created in UTC. If null, uses current UTC time.</param>
    public void SetCreatedAudit(string? createdBy, DateTime? createdAtUtc = null)
    {
        CreatedBy = createdBy;
        CreatedAtUtc = createdAtUtc ?? DateTime.UtcNow;
    }

    /// <summary>
    /// Sets the update audit information.
    /// </summary>
    /// <param name="updatedBy">The identifier of the user who updated the entity.</param>
    /// <param name="updatedAtUtc">The date and time when the entity was updated in UTC. If null, uses current UTC time.</param>
    public void SetUpdatedAudit(string? updatedBy, DateTime? updatedAtUtc = null)
    {
        UpdatedBy = updatedBy;
        UpdatedAtUtc = updatedAtUtc ?? DateTime.UtcNow;
    }
}
