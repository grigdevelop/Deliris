namespace Deliris.BuildingBlocks.Application.Exceptions;

/// <summary>
/// Exception thrown when a user is forbidden from performing an operation.
/// </summary>
public sealed class ForbiddenException : ApplicationException
{
    /// <summary>
    /// Initializes a new instance of the <see cref="ForbiddenException"/> class.
    /// </summary>
    /// <param name="message">The error message that explains the reason for the exception.</param>
    public ForbiddenException(string message)
        : base(message, "FORBIDDEN")
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="ForbiddenException"/> class.
    /// </summary>
    public ForbiddenException()
        : base("You do not have permission to access this resource.", "FORBIDDEN")
    {
    }
}
