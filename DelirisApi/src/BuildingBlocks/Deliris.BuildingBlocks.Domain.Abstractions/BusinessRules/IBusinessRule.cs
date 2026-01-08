namespace Deliris.BuildingBlocks.Domain.Abstractions.BusinessRules;

/// <summary>
/// Interface for business rules that can be validated.
/// Business rules encapsulate domain logic and invariants.
/// </summary>
public interface IBusinessRule
{
    /// <summary>
    /// Gets the name of the business rule.
    /// </summary>
    string Name { get; }

    /// <summary>
    /// Gets the error message when the rule is violated.
    /// </summary>
    string Message { get; }

    /// <summary>
    /// Checks if the business rule is satisfied.
    /// </summary>
    /// <returns>true if the rule is satisfied; otherwise, false.</returns>
    bool IsSatisfied();
}
