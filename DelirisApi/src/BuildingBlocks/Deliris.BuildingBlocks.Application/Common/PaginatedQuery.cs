namespace Deliris.BuildingBlocks.Application.Common;

/// <summary>
/// Base class for paginated queries.
/// </summary>
/// <typeparam name="TResponse">The type of the response.</typeparam>
public abstract record PaginatedQuery<TResponse> : BaseQuery<TResponse>
{
    /// <summary>
    /// Gets the page number (1-based).
    /// </summary>
    public int PageNumber { get; init; } = 1;

    /// <summary>
    /// Gets the page size.
    /// </summary>
    public int PageSize { get; init; } = 10;

    /// <summary>
    /// Gets the search term for filtering.
    /// </summary>
    public string? SearchTerm { get; init; }

    /// <summary>
    /// Gets the sort column.
    /// </summary>
    public string? SortColumn { get; init; }

    /// <summary>
    /// Gets the sort direction (asc or desc).
    /// </summary>
    public string? SortDirection { get; init; }
}
