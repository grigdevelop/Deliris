namespace Deliris.BuildingBlocks.Application.Extensions;

/// <summary>
/// Extension methods for Result pattern.
/// </summary>
public static class ResultExtensions
{
    /// <summary>
    /// Executes a function if the result is successful.
    /// </summary>
    /// <typeparam name="T">The type of the result value.</typeparam>
    /// <param name="result">The result.</param>
    /// <param name="onSuccess">The function to execute on success.</param>
    /// <returns>The result.</returns>
    public static Result<T> OnSuccess<T>(this Result<T> result, Action<T> onSuccess)
    {
        if (result.IsSuccess)
        {
            onSuccess(result.Value);
        }

        return result;
    }

    /// <summary>
    /// Executes a function if the result is a failure.
    /// </summary>
    /// <typeparam name="T">The type of the result value.</typeparam>
    /// <param name="result">The result.</param>
    /// <param name="onFailure">The function to execute on failure.</param>
    /// <returns>The result.</returns>
    public static Result<T> OnFailure<T>(this Result<T> result, Action<string> onFailure)
    {
        if (result.IsFailure)
        {
            onFailure(result.Error);
        }

        return result;
    }

    /// <summary>
    /// Maps the result value to a new type.
    /// </summary>
    /// <typeparam name="TIn">The input type.</typeparam>
    /// <typeparam name="TOut">The output type.</typeparam>
    /// <param name="result">The result.</param>
    /// <param name="mapper">The mapping function.</param>
    /// <returns>A new result with the mapped value.</returns>
    public static Result<TOut> Map<TIn, TOut>(this Result<TIn> result, Func<TIn, TOut> mapper)
    {
        return result.IsSuccess
            ? Result.Success(mapper(result.Value))
            : Result.Failure<TOut>(result.Error);
    }

    /// <summary>
    /// Binds the result to a new result-returning function.
    /// </summary>
    /// <typeparam name="TIn">The input type.</typeparam>
    /// <typeparam name="TOut">The output type.</typeparam>
    /// <param name="result">The result.</param>
    /// <param name="binder">The binding function.</param>
    /// <returns>The result of the binding function or a failure result.</returns>
    public static Result<TOut> Bind<TIn, TOut>(this Result<TIn> result, Func<TIn, Result<TOut>> binder)
    {
        return result.IsSuccess
            ? binder(result.Value)
            : Result.Failure<TOut>(result.Error);
    }

    /// <summary>
    /// Matches the result to one of two functions based on success or failure.
    /// </summary>
    /// <typeparam name="T">The type of the result value.</typeparam>
    /// <typeparam name="TOut">The output type.</typeparam>
    /// <param name="result">The result.</param>
    /// <param name="onSuccess">The function to execute on success.</param>
    /// <param name="onFailure">The function to execute on failure.</param>
    /// <returns>The output of the matched function.</returns>
    public static TOut Match<T, TOut>(
        this Result<T> result,
        Func<T, TOut> onSuccess,
        Func<string, TOut> onFailure)
    {
        return result.IsSuccess
            ? onSuccess(result.Value)
            : onFailure(result.Error);
    }

    /// <summary>
    /// Converts a Result to a Result{T}.
    /// </summary>
    /// <typeparam name="T">The type of the value.</typeparam>
    /// <param name="result">The result.</param>
    /// <param name="value">The value to use if successful.</param>
    /// <returns>A Result{T}.</returns>
    public static Result<T> ToResult<T>(this Result result, T value)
    {
        return result.IsSuccess
            ? Result.Success(value)
            : Result.Failure<T>(result.Error);
    }
}
