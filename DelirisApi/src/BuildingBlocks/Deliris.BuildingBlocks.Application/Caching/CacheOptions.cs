namespace Deliris.BuildingBlocks.Application.Caching;

/// <summary>
/// Options for caching.
/// </summary>
public sealed class CacheOptions
{
    /// <summary>
    /// Gets or sets the absolute expiration time relative to now.
    /// </summary>
    public TimeSpan? AbsoluteExpirationRelativeToNow { get; set; }

    /// <summary>
    /// Gets or sets the sliding expiration time.
    /// </summary>
    public TimeSpan? SlidingExpiration { get; set; }

    /// <summary>
    /// Creates default cache options with 5 minutes sliding expiration.
    /// </summary>
    /// <returns>Default cache options.</returns>
    public static CacheOptions Default => new()
    {
        SlidingExpiration = TimeSpan.FromMinutes(5)
    };

    /// <summary>
    /// Creates cache options with specified absolute expiration.
    /// </summary>
    /// <param name="absoluteExpiration">The absolute expiration time.</param>
    /// <returns>Cache options with absolute expiration.</returns>
    public static CacheOptions WithAbsoluteExpiration(TimeSpan absoluteExpiration) => new()
    {
        AbsoluteExpirationRelativeToNow = absoluteExpiration
    };

    /// <summary>
    /// Creates cache options with specified sliding expiration.
    /// </summary>
    /// <param name="slidingExpiration">The sliding expiration time.</param>
    /// <returns>Cache options with sliding expiration.</returns>
    public static CacheOptions WithSlidingExpiration(TimeSpan slidingExpiration) => new()
    {
        SlidingExpiration = slidingExpiration
    };
}
