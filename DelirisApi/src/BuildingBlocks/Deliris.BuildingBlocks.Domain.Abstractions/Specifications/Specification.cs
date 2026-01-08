using System.Linq.Expressions;

namespace Deliris.BuildingBlocks.Domain.Abstractions.Specifications;

/// <summary>
/// Base class for specifications following the Specification pattern.
/// Provides a fluent API for building complex query logic.
/// </summary>
/// <typeparam name="T">The type of entity the specification applies to.</typeparam>
public abstract class Specification<T> : ISpecification<T>
{
    /// <summary>
    /// Gets the criteria expression that defines the specification.
    /// </summary>
    public Expression<Func<T, bool>>? Criteria { get; private set; }

    /// <summary>
    /// Gets the collection of include expressions for eager loading.
    /// </summary>
    public List<Expression<Func<T, object>>> Includes { get; } = new();

    /// <summary>
    /// Gets the collection of include string expressions for eager loading.
    /// </summary>
    public List<string> IncludeStrings { get; } = new();

    /// <summary>
    /// Gets the order by expression for ascending ordering.
    /// </summary>
    public Expression<Func<T, object>>? OrderBy { get; private set; }

    /// <summary>
    /// Gets the order by expression for descending ordering.
    /// </summary>
    public Expression<Func<T, object>>? OrderByDescending { get; private set; }

    /// <summary>
    /// Gets the group by expression.
    /// </summary>
    public Expression<Func<T, object>>? GroupBy { get; private set; }

    /// <summary>
    /// Gets the number of items to take for pagination.
    /// </summary>
    public int? Take { get; private set; }

    /// <summary>
    /// Gets the number of items to skip for pagination.
    /// </summary>
    public int? Skip { get; private set; }

    /// <summary>
    /// Gets a value indicating whether tracking is enabled for the query.
    /// </summary>
    public bool IsTrackingEnabled { get; private set; } = true;

    /// <summary>
    /// Gets a value indicating whether the query should split into multiple queries.
    /// </summary>
    public bool IsSplitQuery { get; private set; }

    /// <summary>
    /// Initializes a new instance of the <see cref="Specification{T}"/> class.
    /// </summary>
    protected Specification()
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="Specification{T}"/> class with criteria.
    /// </summary>
    /// <param name="criteria">The criteria expression.</param>
    protected Specification(Expression<Func<T, bool>> criteria)
    {
        Criteria = criteria;
    }

    /// <summary>
    /// Adds a criteria expression to the specification.
    /// </summary>
    /// <param name="criteria">The criteria expression to add.</param>
    protected void AddCriteria(Expression<Func<T, bool>> criteria)
    {
        Criteria = criteria;
    }

    /// <summary>
    /// Adds an include expression for eager loading.
    /// </summary>
    /// <param name="includeExpression">The include expression to add.</param>
    protected void AddInclude(Expression<Func<T, object>> includeExpression)
    {
        Includes.Add(includeExpression);
    }

    /// <summary>
    /// Adds an include string for eager loading.
    /// </summary>
    /// <param name="includeString">The include string to add.</param>
    protected void AddInclude(string includeString)
    {
        IncludeStrings.Add(includeString);
    }

    /// <summary>
    /// Adds an order by expression for ascending ordering.
    /// </summary>
    /// <param name="orderByExpression">The order by expression to add.</param>
    protected void AddOrderBy(Expression<Func<T, object>> orderByExpression)
    {
        OrderBy = orderByExpression;
    }

    /// <summary>
    /// Adds an order by expression for descending ordering.
    /// </summary>
    /// <param name="orderByDescExpression">The order by descending expression to add.</param>
    protected void AddOrderByDescending(Expression<Func<T, object>> orderByDescExpression)
    {
        OrderByDescending = orderByDescExpression;
    }

    /// <summary>
    /// Adds a group by expression.
    /// </summary>
    /// <param name="groupByExpression">The group by expression to add.</param>
    protected void AddGroupBy(Expression<Func<T, object>> groupByExpression)
    {
        GroupBy = groupByExpression;
    }

    /// <summary>
    /// Applies pagination to the specification.
    /// </summary>
    /// <param name="skip">The number of items to skip.</param>
    /// <param name="take">The number of items to take.</param>
    protected void ApplyPaging(int skip, int take)
    {
        Skip = skip;
        Take = take;
    }

    /// <summary>
    /// Disables change tracking for the query.
    /// </summary>
    protected void AsNoTracking()
    {
        IsTrackingEnabled = false;
    }

    /// <summary>
    /// Enables split query mode for the query.
    /// </summary>
    protected void AsSplitQuery()
    {
        IsSplitQuery = true;
    }
}
