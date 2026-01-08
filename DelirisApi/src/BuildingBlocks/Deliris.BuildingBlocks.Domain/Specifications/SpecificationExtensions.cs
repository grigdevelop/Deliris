using System.Linq.Expressions;

namespace Deliris.BuildingBlocks.Domain.Specifications;

/// <summary>
/// Extension methods for combining specifications using logical operators.
/// </summary>
public static class SpecificationExtensions
{
    /// <param name="left">The left specification.</param>
    /// <typeparam name="T">The type of entity.</typeparam>
    extension<T>(ISpecification<T> left)
    {
        /// <summary>
        /// Combines two specifications using the AND operator.
        /// </summary>
        /// <param name="right">The right specification.</param>
        /// <returns>A new specification that is the logical AND of the two specifications.</returns>
        public ISpecification<T> And(ISpecification<T> right)
        {
            return new AndSpecification<T>(left, right);
        }

        /// <summary>
        /// Combines two specifications using the OR operator.
        /// </summary>
        /// <param name="right">The right specification.</param>
        /// <returns>A new specification that is the logical OR of the two specifications.</returns>
        public ISpecification<T> Or(ISpecification<T> right)
        {
            return new OrSpecification<T>(left, right);
        }

        /// <summary>
        /// Negates a specification using the NOT operator.
        /// </summary>
        /// <returns>A new specification that is the logical NOT of the original specification.</returns>
        public ISpecification<T> Not()
        {
            return new NotSpecification<T>(left);
        }
    }
}

internal sealed class AndSpecification<T> : Abstractions.Specifications.Specification<T>
{
    private readonly ISpecification<T> _left;
    private readonly ISpecification<T> _right;

    public AndSpecification(ISpecification<T> left, ISpecification<T> right)
    {
        _left = left;
        _right = right;

        if (_left.Criteria != null && _right.Criteria != null)
        {
            var parameter = Expression.Parameter(typeof(T), "x");
            var leftExpression = ReplaceParameter(_left.Criteria.Body, _left.Criteria.Parameters[0], parameter);
            var rightExpression = ReplaceParameter(_right.Criteria.Body, _right.Criteria.Parameters[0], parameter);
            var andExpression = Expression.AndAlso(leftExpression, rightExpression);
            AddCriteria(Expression.Lambda<Func<T, bool>>(andExpression, parameter));
        }
    }

    private static Expression ReplaceParameter(Expression expression, ParameterExpression oldParameter, ParameterExpression newParameter)
    {
        return new ParameterReplacer(oldParameter, newParameter).Visit(expression)!;
    }
}

internal sealed class OrSpecification<T> : Abstractions.Specifications.Specification<T>
{
    private readonly ISpecification<T> _left;
    private readonly ISpecification<T> _right;

    public OrSpecification(ISpecification<T> left, ISpecification<T> right)
    {
        _left = left;
        _right = right;

        if (_left.Criteria != null && _right.Criteria != null)
        {
            var parameter = Expression.Parameter(typeof(T), "x");
            var leftExpression = ReplaceParameter(_left.Criteria.Body, _left.Criteria.Parameters[0], parameter);
            var rightExpression = ReplaceParameter(_right.Criteria.Body, _right.Criteria.Parameters[0], parameter);
            var orExpression = Expression.OrElse(leftExpression, rightExpression);
            AddCriteria(Expression.Lambda<Func<T, bool>>(orExpression, parameter));
        }
    }

    private static Expression ReplaceParameter(Expression expression, ParameterExpression oldParameter, ParameterExpression newParameter)
    {
        return new ParameterReplacer(oldParameter, newParameter).Visit(expression)!;
    }
}

internal sealed class NotSpecification<T> : Abstractions.Specifications.Specification<T>
{
    public NotSpecification(ISpecification<T> specification)
    {
        if (specification.Criteria != null)
        {
            var parameter = Expression.Parameter(typeof(T), "x");
            var expression = ReplaceParameter(specification.Criteria.Body, specification.Criteria.Parameters[0], parameter);
            var notExpression = Expression.Not(expression);
            AddCriteria(Expression.Lambda<Func<T, bool>>(notExpression, parameter));
        }
    }

    private static Expression ReplaceParameter(Expression expression, ParameterExpression oldParameter, ParameterExpression newParameter)
    {
        return new ParameterReplacer(oldParameter, newParameter).Visit(expression)!;
    }
}

internal sealed class ParameterReplacer : ExpressionVisitor
{
    private readonly ParameterExpression _oldParameter;
    private readonly ParameterExpression _newParameter;

    public ParameterReplacer(ParameterExpression oldParameter, ParameterExpression newParameter)
    {
        _oldParameter = oldParameter;
        _newParameter = newParameter;
    }

    protected override Expression VisitParameter(ParameterExpression node)
    {
        return node == _oldParameter ? _newParameter : base.VisitParameter(node);
    }
}
