namespace Deliris.BuildingBlocks.Application.Common;

/// <summary>
/// Represents a paginated result with items and pagination metadata.
/// </summary>
/// <typeparam name="T">The type of items in the result.</typeparam>
public sealed class PaginatedResult<T>
{
    /// <summary>
    /// Gets the items in the current page.
    /// </summary>
    public IReadOnlyList<T> Items { get; }

    /// <summary>
    /// Gets the current page number (1-based).
    /// </summary>
    public int PageNumber { get; }

    /// <summary>
    /// Gets the page size.
    /// </summary>
    public int PageSize { get; }

    /// <summary>
    /// Gets the total number of items across all pages.
    /// </summary>
    public int TotalCount { get; }

    /// <summary>
    /// Gets the total number of pages.
    /// </summary>
    public int TotalPages { get; }

    /// <summary>
    /// Gets a value indicating whether there is a previous page.
    /// </summary>
    public bool HasPreviousPage => PageNumber > 1;

    /// <summary>
    /// Gets a value indicating whether there is a next page.
    /// </summary>
    public bool HasNextPage => PageNumber < TotalPages;

    /// <summary>
    /// Initializes a new instance of the <see cref="PaginatedResult{T}"/> class.
    /// </summary>
    /// <param name="items">The items in the current page.</param>
    /// <param name="pageNumber">The current page number (1-based).</param>
    /// <param name="pageSize">The page size.</param>
    /// <param name="totalCount">The total number of items across all pages.</param>
    public PaginatedResult(IReadOnlyList<T> items, int pageNumber, int pageSize, int totalCount)
    {
        Items = items;
        PageNumber = pageNumber;
        PageSize = pageSize;
        TotalCount = totalCount;
        TotalPages = (int)Math.Ceiling(totalCount / (double)pageSize);
    }

    /// <summary>
    /// Creates an empty paginated result.
    /// </summary>
    /// <param name="pageNumber">The current page number (1-based).</param>
    /// <param name="pageSize">The page size.</param>
    /// <returns>An empty paginated result.</returns>
    public static PaginatedResult<T> Empty(int pageNumber = 1, int pageSize = 10)
    {
        return new PaginatedResult<T>(Array.Empty<T>(), pageNumber, pageSize, 0);
    }

    /// <summary>
    /// Creates a paginated result from a PagedList.
    /// </summary>
    /// <param name="pagedList">The paged list.</param>
    /// <returns>A paginated result.</returns>
    public static PaginatedResult<T> FromPagedList(PagedList<T> pagedList)
    {
        return new PaginatedResult<T>(
            pagedList.Items,
            pagedList.PageNumber,
            pagedList.PageSize,
            pagedList.TotalCount);
    }
}
