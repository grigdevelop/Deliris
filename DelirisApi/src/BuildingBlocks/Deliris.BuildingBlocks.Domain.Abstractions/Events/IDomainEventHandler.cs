namespace Deliris.BuildingBlocks.Domain.Abstractions.Events;

/// <summary>
/// Interface for domain event handlers.
/// Handlers process domain events asynchronously.
/// </summary>
/// <typeparam name="TDomainEvent">The type of domain event to handle.</typeparam>
public interface IDomainEventHandler<in TDomainEvent> where TDomainEvent : IDomainEvent
{
    /// <summary>
    /// Handles the specified domain event.
    /// </summary>
    /// <param name="domainEvent">The domain event to handle.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    Task Handle(TDomainEvent domainEvent, CancellationToken cancellationToken = default);
}
