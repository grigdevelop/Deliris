namespace Deliris.BuildingBlocks.Application.Validation;

/// <summary>
/// Extension methods for FluentValidation validators.
/// </summary>
public static class AbstractValidatorExtensions
{
    /// <summary>
    /// Validates that a GUID is not empty.
    /// </summary>
    /// <typeparam name="T">The type being validated.</typeparam>
    /// <param name="ruleBuilder">The rule builder.</param>
    /// <returns>The rule builder options.</returns>
    public static IRuleBuilderOptions<T, Guid> NotEmpty<T>(this IRuleBuilder<T, Guid> ruleBuilder)
    {
        return ruleBuilder
            .Must(id => id != Guid.Empty)
            .WithMessage("'{PropertyName}' must not be empty.");
    }

    /// <summary>
    /// Validates that a nullable GUID is not empty if it has a value.
    /// </summary>
    /// <typeparam name="T">The type being validated.</typeparam>
    /// <param name="ruleBuilder">The rule builder.</param>
    /// <returns>The rule builder options.</returns>
    public static IRuleBuilderOptions<T, Guid?> NotEmpty<T>(this IRuleBuilder<T, Guid?> ruleBuilder)
    {
        return ruleBuilder
            .Must(id => !id.HasValue || id.Value != Guid.Empty)
            .WithMessage("'{PropertyName}' must not be empty.");
    }

    /// <summary>
    /// Validates that a string is a valid email address.
    /// </summary>
    /// <typeparam name="T">The type being validated.</typeparam>
    /// <param name="ruleBuilder">The rule builder.</param>
    /// <returns>The rule builder options.</returns>
    public static IRuleBuilderOptions<T, string> IsValidEmail<T>(this IRuleBuilder<T, string> ruleBuilder)
    {
        return ruleBuilder
            .EmailAddress()
            .WithMessage("'{PropertyName}' is not a valid email address.");
    }

    /// <summary>
    /// Validates that a collection is not empty.
    /// </summary>
    /// <typeparam name="T">The type being validated.</typeparam>
    /// <typeparam name="TElement">The element type of the collection.</typeparam>
    /// <param name="ruleBuilder">The rule builder.</param>
    /// <returns>The rule builder options.</returns>
    public static IRuleBuilderOptions<T, IEnumerable<TElement>> NotEmpty<T, TElement>(
        this IRuleBuilder<T, IEnumerable<TElement>> ruleBuilder)
    {
        return ruleBuilder
            .Must(collection => collection != null && collection.Any())
            .WithMessage("'{PropertyName}' must not be empty.");
    }

    /// <summary>
    /// Validates that a value is within a specified range.
    /// </summary>
    /// <typeparam name="T">The type being validated.</typeparam>
    /// <typeparam name="TProperty">The property type.</typeparam>
    /// <param name="ruleBuilder">The rule builder.</param>
    /// <param name="min">The minimum value.</param>
    /// <param name="max">The maximum value.</param>
    /// <returns>The rule builder options.</returns>
    public static IRuleBuilderOptions<T, TProperty> InRange<T, TProperty>(
        this IRuleBuilder<T, TProperty> ruleBuilder,
        TProperty min,
        TProperty max) where TProperty : IComparable<TProperty>, IComparable
    {
        return ruleBuilder
            .Must(value => value.CompareTo(min) >= 0 && value.CompareTo(max) <= 0)
            .WithMessage($"'{{PropertyName}}' must be between {min} and {max}.");
    }
}
