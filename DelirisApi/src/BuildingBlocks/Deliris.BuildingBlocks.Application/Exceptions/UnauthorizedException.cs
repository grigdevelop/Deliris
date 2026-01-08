namespace Deliris.BuildingBlocks.Application.Exceptions;

/// <summary>
/// Exception thrown when a user is not authorized to perform an operation.
/// </summary>
public sealed class UnauthorizedException : ApplicationException
{
    /// <summary>
    /// Initializes a new instance of the <see cref="UnauthorizedException"/> class.
    /// </summary>
    /// <param name="message">The error message that explains the reason for the exception.</param>
    public UnauthorizedException(string message)
        : base(message, "UNAUTHORIZED")
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="UnauthorizedException"/> class.
    /// </summary>
    public UnauthorizedException()
        : base("You are not authorized to perform this operation.", "UNAUTHORIZED")
    {
    }
}
