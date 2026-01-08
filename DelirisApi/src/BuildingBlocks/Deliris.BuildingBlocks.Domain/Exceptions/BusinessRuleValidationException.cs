namespace Deliris.BuildingBlocks.Domain.Exceptions;

/// <summary>
/// Exception thrown when a business rule validation fails.
/// </summary>
public sealed class BusinessRuleValidationException : Abstractions.Exceptions.DomainException
{
    /// <summary>
    /// Gets the name of the business rule that was violated.
    /// </summary>
    public string RuleName { get; }

    /// <summary>
    /// Initializes a new instance of the <see cref="BusinessRuleValidationException"/> class.
    /// </summary>
    /// <param name="ruleName">The name of the business rule that was violated.</param>
    /// <param name="message">The error message that explains the reason for the exception.</param>
    public BusinessRuleValidationException(string ruleName, string message)
        : base(message, "BUSINESS_RULE_VIOLATION")
    {
        RuleName = ruleName;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="BusinessRuleValidationException"/> class.
    /// </summary>
    /// <param name="ruleName">The name of the business rule that was violated.</param>
    /// <param name="message">The error message that explains the reason for the exception.</param>
    /// <param name="innerException">The exception that is the cause of the current exception.</param>
    public BusinessRuleValidationException(string ruleName, string message, Exception innerException)
        : base(message, "BUSINESS_RULE_VIOLATION", innerException)
    {
        RuleName = ruleName;
    }
}
