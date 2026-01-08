using Deliris.BuildingBlocks.Domain.Exceptions;

namespace Deliris.BuildingBlocks.Domain.Primitives;

/// <summary>
/// Utility class for validating business rules.
/// </summary>
public static class BusinessRuleValidator
{
    /// <summary>
    /// Checks if a business rule is satisfied and throws an exception if not.
    /// </summary>
    /// <param name="rule">The business rule to check.</param>
    /// <exception cref="BusinessRuleValidationException">Thrown when the business rule is not satisfied.</exception>
    public static void CheckRule(IBusinessRule rule)
    {
        ArgumentNullException.ThrowIfNull(rule);

        if (!rule.IsSatisfied())
        {
            throw new BusinessRuleValidationException(rule.Name, rule.Message);
        }
    }

    /// <summary>
    /// Checks multiple business rules and throws an exception if any are not satisfied.
    /// </summary>
    /// <param name="rules">The business rules to check.</param>
    /// <exception cref="BusinessRuleValidationException">Thrown when any business rule is not satisfied.</exception>
    public static void CheckRules(params IBusinessRule[] rules)
    {
        ArgumentNullException.ThrowIfNull(rules);

        foreach (var rule in rules)
        {
            CheckRule(rule);
        }
    }

    /// <summary>
    /// Validates a business rule and returns whether it is satisfied.
    /// </summary>
    /// <param name="rule">The business rule to validate.</param>
    /// <returns>true if the rule is satisfied; otherwise, false.</returns>
    public static bool Validate(IBusinessRule rule)
    {
        ArgumentNullException.ThrowIfNull(rule);
        return rule.IsSatisfied();
    }
}
