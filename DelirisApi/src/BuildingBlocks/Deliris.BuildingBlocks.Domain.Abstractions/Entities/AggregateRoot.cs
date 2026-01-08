using Deliris.BuildingBlocks.Domain.Abstractions.Events;

namespace Deliris.BuildingBlocks.Domain.Abstractions.Entities;

/// <summary>
/// Base class for aggregate roots following DDD principles.
/// An aggregate root is an entity that serves as the entry point to an aggregate
/// and maintains consistency boundaries within the aggregate.
/// </summary>
/// <typeparam name="TId">The type of the entity identifier.</typeparam>
public abstract class AggregateRoot<TId> : Entity<TId>, IAggregateRoot where TId : notnull
{
    private readonly List<IDomainEvent> _domainEvents = new();

    /// <summary>
    /// Gets the collection of domain events raised by this aggregate root.
    /// </summary>
    public IReadOnlyCollection<IDomainEvent> DomainEvents => _domainEvents.AsReadOnly();

    /// <summary>
    /// Initializes a new instance of the <see cref="AggregateRoot{TId}"/> class.
    /// </summary>
    /// <param name="id">The unique identifier of the aggregate root.</param>
    protected AggregateRoot(TId id) : base(id)
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="AggregateRoot{TId}"/> class.
    /// Parameterless constructor for ORM support.
    /// </summary>
    protected AggregateRoot() : base()
    {
    }

    /// <summary>
    /// Raises a domain event by adding it to the collection of domain events.
    /// </summary>
    /// <param name="domainEvent">The domain event to raise.</param>
    protected void RaiseDomainEvent(IDomainEvent domainEvent)
    {
        ArgumentNullException.ThrowIfNull(domainEvent);
        _domainEvents.Add(domainEvent);
    }

    /// <summary>
    /// Clears all domain events from the aggregate root.
    /// This is typically called after events have been dispatched.
    /// </summary>
    public void ClearDomainEvents()
    {
        _domainEvents.Clear();
    }
}
