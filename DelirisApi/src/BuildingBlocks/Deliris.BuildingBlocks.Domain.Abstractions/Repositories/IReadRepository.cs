using System.Linq.Expressions;
using Deliris.BuildingBlocks.Domain.Abstractions.Entities;
using Deliris.BuildingBlocks.Domain.Abstractions.Specifications;

namespace Deliris.BuildingBlocks.Domain.Abstractions.Repositories;

/// <summary>
/// Interface for read-only repository operations.
/// Provides methods for querying entities without modification capabilities.
/// </summary>
/// <typeparam name="TEntity">The type of the entity.</typeparam>
/// <typeparam name="TId">The type of the entity identifier.</typeparam>
public interface IReadRepository<TEntity, TId> 
    where TEntity : Entity<TId> 
    where TId : notnull
{
    /// <summary>
    /// Gets an entity by its identifier.
    /// </summary>
    /// <param name="id">The entity identifier.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>The entity if found; otherwise, null.</returns>
    Task<TEntity?> GetByIdAsync(TId id, CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets all entities.
    /// </summary>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>A collection of all entities.</returns>
    Task<IReadOnlyList<TEntity>> GetAllAsync(CancellationToken cancellationToken = default);

    /// <summary>
    /// Finds entities that match the specified predicate.
    /// </summary>
    /// <param name="predicate">The predicate to filter entities.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>A collection of entities that match the predicate.</returns>
    Task<IReadOnlyList<TEntity>> FindAsync(
        Expression<Func<TEntity, bool>> predicate, 
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Finds entities that satisfy the specified specification.
    /// </summary>
    /// <param name="specification">The specification to apply.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>A collection of entities that satisfy the specification.</returns>
    Task<IReadOnlyList<TEntity>> FindAsync(
        ISpecification<TEntity> specification, 
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets the first entity that matches the specified predicate.
    /// </summary>
    /// <param name="predicate">The predicate to filter entities.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>The first entity that matches the predicate if found; otherwise, null.</returns>
    Task<TEntity?> FirstOrDefaultAsync(
        Expression<Func<TEntity, bool>> predicate, 
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets the single entity that matches the specified predicate.
    /// </summary>
    /// <param name="predicate">The predicate to filter entities.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>The single entity that matches the predicate if found; otherwise, null.</returns>
    /// <exception cref="InvalidOperationException">Thrown when more than one entity matches the predicate.</exception>
    Task<TEntity?> SingleOrDefaultAsync(
        Expression<Func<TEntity, bool>> predicate, 
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Determines whether any entity matches the specified predicate.
    /// </summary>
    /// <param name="predicate">The predicate to filter entities.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>true if any entity matches the predicate; otherwise, false.</returns>
    Task<bool> AnyAsync(
        Expression<Func<TEntity, bool>> predicate, 
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets the count of entities that match the specified predicate.
    /// </summary>
    /// <param name="predicate">The predicate to filter entities.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>The count of entities that match the predicate.</returns>
    Task<int> CountAsync(
        Expression<Func<TEntity, bool>> predicate, 
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets a queryable collection of entities for advanced querying.
    /// </summary>
    /// <returns>An IQueryable of entities.</returns>
    IQueryable<TEntity> AsQueryable();
}
