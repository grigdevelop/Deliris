namespace Deliris.BuildingBlocks.Domain.Abstractions.Common;

/// <summary>
/// Represents a paged list of items with pagination metadata.
/// </summary>
/// <typeparam name="T">The type of items in the list.</typeparam>
public sealed class PagedList<T>
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
    /// Initializes a new instance of the <see cref="PagedList{T}"/> class.
    /// </summary>
    /// <param name="items">The items in the current page.</param>
    /// <param name="pageNumber">The current page number (1-based).</param>
    /// <param name="pageSize">The page size.</param>
    /// <param name="totalCount">The total number of items across all pages.</param>
    public PagedList(IReadOnlyList<T> items, int pageNumber, int pageSize, int totalCount)
    {
        if (pageNumber < 1)
            throw new ArgumentOutOfRangeException(nameof(pageNumber), "Page number must be greater than or equal to 1.");

        if (pageSize < 1)
            throw new ArgumentOutOfRangeException(nameof(pageSize), "Page size must be greater than or equal to 1.");

        if (totalCount < 0)
            throw new ArgumentOutOfRangeException(nameof(totalCount), "Total count must be greater than or equal to 0.");

        Items = items ?? throw new ArgumentNullException(nameof(items));
        PageNumber = pageNumber;
        PageSize = pageSize;
        TotalCount = totalCount;
        TotalPages = (int)Math.Ceiling(totalCount / (double)pageSize);
    }

    /// <summary>
    /// Creates an empty paged list.
    /// </summary>
    /// <param name="pageNumber">The current page number (1-based).</param>
    /// <param name="pageSize">The page size.</param>
    /// <returns>An empty paged list.</returns>
    public static PagedList<T> Empty(int pageNumber = 1, int pageSize = 10)
    {
        return new PagedList<T>(Array.Empty<T>(), pageNumber, pageSize, 0);
    }

    /// <summary>
    /// Creates a paged list from a collection of items.
    /// </summary>
    /// <param name="items">The items to paginate.</param>
    /// <param name="pageNumber">The current page number (1-based).</param>
    /// <param name="pageSize">The page size.</param>
    /// <returns>A paged list containing the specified page of items.</returns>
    public static PagedList<T> Create(IEnumerable<T> items, int pageNumber, int pageSize)
    {
        var itemsList = items.ToList();
        var totalCount = itemsList.Count;
        var pagedItems = itemsList
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToList();

        return new PagedList<T>(pagedItems, pageNumber, pageSize, totalCount);
    }
}
