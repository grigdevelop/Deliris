namespace Deliris.BuildingBlocks.Domain.Abstractions.Exceptions;

/// <summary>
/// Base exception class for domain-related exceptions.
/// Represents violations of business rules or domain invariants.
/// </summary>
public abstract class DomainException : Exception
{
    /// <summary>
    /// Gets the error code associated with this domain exception.
    /// </summary>
    public string ErrorCode { get; }

    /// <summary>
    /// Initializes a new instance of the <see cref="DomainException"/> class.
    /// </summary>
    /// <param name="message">The error message that explains the reason for the exception.</param>
    /// <param name="errorCode">The error code associated with this exception.</param>
    protected DomainException(string message, string errorCode)
        : base(message)
    {
        ErrorCode = errorCode;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="DomainException"/> class.
    /// </summary>
    /// <param name="message">The error message that explains the reason for the exception.</param>
    /// <param name="errorCode">The error code associated with this exception.</param>
    /// <param name="innerException">The exception that is the cause of the current exception.</param>
    protected DomainException(string message, string errorCode, Exception innerException)
        : base(message, innerException)
    {
        ErrorCode = errorCode;
    }
}
