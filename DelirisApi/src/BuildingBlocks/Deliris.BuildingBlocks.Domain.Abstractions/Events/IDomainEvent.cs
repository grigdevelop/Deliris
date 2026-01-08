namespace Deliris.BuildingBlocks.Domain.Abstractions.Events;

/// <summary>
/// Marker interface for domain events.
/// Domain events represent something that happened in the domain that domain experts care about.
/// </summary>
public interface IDomainEvent
{
    /// <summary>
    /// Gets the unique identifier of the domain event.
    /// </summary>
    Guid Id { get; }

    /// <summary>
    /// Gets the date and time when the event occurred in UTC.
    /// </summary>
    DateTime OccurredOnUtc { get; }
}
