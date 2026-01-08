namespace Deliris.BuildingBlocks.Domain.Abstractions.Common;

/// <summary>
/// Represents a domain error with a code and message.
/// </summary>
public sealed record Error
{
    /// <summary>
    /// Gets the error code.
    /// </summary>
    public string Code { get; }

    /// <summary>
    /// Gets the error message.
    /// </summary>
    public string Message { get; }

    /// <summary>
    /// Gets a value indicating whether this is a null error (no error).
    /// </summary>
    public bool IsNullError => this == None;

    /// <summary>
    /// Represents no error.
    /// </summary>
    public static readonly Error None = new(string.Empty, string.Empty);

    /// <summary>
    /// Represents a null value error.
    /// </summary>
    public static readonly Error NullValue = new("Error.NullValue", "The specified value is null.");

    /// <summary>
    /// Initializes a new instance of the <see cref="Error"/> class.
    /// </summary>
    /// <param name="code">The error code.</param>
    /// <param name="message">The error message.</param>
    public Error(string code, string message)
    {
        Code = code;
        Message = message;
    }

    /// <summary>
    /// Implicitly converts an error to a string.
    /// </summary>
    /// <param name="error">The error.</param>
    public static implicit operator string(Error error) => error.Code;

    /// <summary>
    /// Returns a string representation of the error.
    /// </summary>
    /// <returns>The error code and message.</returns>
    public override string ToString() => $"{Code}: {Message}";
}
