using System.Linq.Expressions;

namespace Deliris.BuildingBlocks.Domain.Abstractions.Specifications;

/// <summary>
/// Interface for the Specification pattern.
/// Encapsulates query logic in a reusable and composable way.
/// </summary>
/// <typeparam name="T">The type of entity the specification applies to.</typeparam>
public interface ISpecification<T>
{
    /// <summary>
    /// Gets the criteria expression that defines the specification.
    /// </summary>
    Expression<Func<T, bool>>? Criteria { get; }

    /// <summary>
    /// Gets the collection of include expressions for eager loading.
    /// </summary>
    List<Expression<Func<T, object>>> Includes { get; }

    /// <summary>
    /// Gets the collection of include string expressions for eager loading.
    /// </summary>
    List<string> IncludeStrings { get; }

    /// <summary>
    /// Gets the order by expression for ascending ordering.
    /// </summary>
    Expression<Func<T, object>>? OrderBy { get; }

    /// <summary>
    /// Gets the order by expression for descending ordering.
    /// </summary>
    Expression<Func<T, object>>? OrderByDescending { get; }

    /// <summary>
    /// Gets the group by expression.
    /// </summary>
    Expression<Func<T, object>>? GroupBy { get; }

    /// <summary>
    /// Gets the number of items to take for pagination.
    /// </summary>
    int? Take { get; }

    /// <summary>
    /// Gets the number of items to skip for pagination.
    /// </summary>
    int? Skip { get; }

    /// <summary>
    /// Gets a value indicating whether tracking is enabled for the query.
    /// </summary>
    bool IsTrackingEnabled { get; }

    /// <summary>
    /// Gets a value indicating whether the query should split into multiple queries.
    /// </summary>
    bool IsSplitQuery { get; }
}
