namespace Deliris.BuildingBlocks.Domain.Abstractions.Events;

/// <summary>
/// Base class for domain events.
/// Domain events are immutable records of something that happened in the domain.
/// </summary>
public abstract record DomainEvent : IDomainEvent
{
    /// <summary>
    /// Gets the unique identifier of the domain event.
    /// </summary>
    public Guid Id { get; }

    /// <summary>
    /// Gets the date and time when the event occurred in UTC.
    /// </summary>
    public DateTime OccurredOnUtc { get; }

    /// <summary>
    /// Initializes a new instance of the <see cref="DomainEvent"/> class.
    /// </summary>
    protected DomainEvent()
    {
        Id = Guid.NewGuid();
        OccurredOnUtc = DateTime.UtcNow;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="DomainEvent"/> class with specified values.
    /// </summary>
    /// <param name="id">The unique identifier of the event.</param>
    /// <param name="occurredOnUtc">The date and time when the event occurred in UTC.</param>
    protected DomainEvent(Guid id, DateTime occurredOnUtc)
    {
        Id = id;
        OccurredOnUtc = occurredOnUtc;
    }
}
