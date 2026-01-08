namespace Deliris.BuildingBlocks.Domain.Tests.Common;

[Trait("Category", "Unit")]
[Trait("Component", "PagedList")]
public class PagedListTests
{
    private readonly Faker _faker = new();

    [Fact]
    public void Constructor_WithValidParameters_ShouldCreatePagedList()
    {
        // Arrange
        var items = new List<string> { "Item1", "Item2", "Item3" };
        var pageNumber = 1;
        var pageSize = 10;
        var totalCount = 25;

        // Act
        var pagedList = new PagedList<string>(items, pageNumber, pageSize, totalCount);

        // Assert
        pagedList.Items.Should().HaveCount(3);
        pagedList.PageNumber.Should().Be(pageNumber);
        pagedList.PageSize.Should().Be(pageSize);
        pagedList.TotalCount.Should().Be(totalCount);
        pagedList.TotalPages.Should().Be(3);
    }

    [Fact]
    public void Constructor_WithPageNumberLessThanOne_ShouldThrowArgumentOutOfRangeException()
    {
        // Arrange
        var items = new List<string> { "Item1" };

        // Act
        Action act = () => new PagedList<string>(items, 0, 10, 100);

        // Assert
        act.Should().Throw<ArgumentOutOfRangeException>()
            .WithMessage("*Page number must be greater than or equal to 1*");
    }

    [Fact]
    public void Constructor_WithPageSizeLessThanOne_ShouldThrowArgumentOutOfRangeException()
    {
        // Arrange
        var items = new List<string> { "Item1" };

        // Act
        Action act = () => new PagedList<string>(items, 1, 0, 100);

        // Assert
        act.Should().Throw<ArgumentOutOfRangeException>()
            .WithMessage("*Page size must be greater than or equal to 1*");
    }

    [Fact]
    public void Constructor_WithNegativeTotalCount_ShouldThrowArgumentOutOfRangeException()
    {
        // Arrange
        var items = new List<string> { "Item1" };

        // Act
        Action act = () => new PagedList<string>(items, 1, 10, -1);

        // Assert
        act.Should().Throw<ArgumentOutOfRangeException>()
            .WithMessage("*Total count must be greater than or equal to 0*");
    }

    [Fact]
    public void Constructor_WithNullItems_ShouldThrowArgumentNullException()
    {
        // Act
        Action act = () => new PagedList<string>(null!, 1, 10, 100);

        // Assert
        act.Should().Throw<ArgumentNullException>();
    }

    [Fact]
    public void HasPreviousPage_OnFirstPage_ShouldReturnFalse()
    {
        // Arrange
        var items = new List<string> { "Item1" };
        var pagedList = new PagedList<string>(items, 1, 10, 100);

        // Act & Assert
        pagedList.HasPreviousPage.Should().BeFalse();
    }

    [Fact]
    public void HasPreviousPage_OnSecondPage_ShouldReturnTrue()
    {
        // Arrange
        var items = new List<string> { "Item1" };
        var pagedList = new PagedList<string>(items, 2, 10, 100);

        // Act & Assert
        pagedList.HasPreviousPage.Should().BeTrue();
    }

    [Fact]
    public void HasNextPage_OnLastPage_ShouldReturnFalse()
    {
        // Arrange
        var items = new List<string> { "Item1" };
        var pagedList = new PagedList<string>(items, 3, 10, 25);

        // Act & Assert
        pagedList.HasNextPage.Should().BeFalse();
    }

    [Fact]
    public void HasNextPage_OnFirstPage_ShouldReturnTrue()
    {
        // Arrange
        var items = new List<string> { "Item1" };
        var pagedList = new PagedList<string>(items, 1, 10, 100);

        // Act & Assert
        pagedList.HasNextPage.Should().BeTrue();
    }

    [Fact]
    public void TotalPages_ShouldBeCalculatedCorrectly()
    {
        // Arrange
        var items = new List<string> { "Item1" };

        // Act
        var pagedList = new PagedList<string>(items, 1, 10, 25);

        // Assert
        pagedList.TotalPages.Should().Be(3);
    }

    [Fact]
    public void Empty_ShouldCreateEmptyPagedList()
    {
        // Act
        var pagedList = PagedList<string>.Empty();

        // Assert
        pagedList.Items.Should().BeEmpty();
        pagedList.PageNumber.Should().Be(1);
        pagedList.PageSize.Should().Be(10);
        pagedList.TotalCount.Should().Be(0);
        pagedList.TotalPages.Should().Be(0);
    }

    [Fact]
    public void Empty_WithCustomParameters_ShouldCreateEmptyPagedList()
    {
        // Act
        var pagedList = PagedList<string>.Empty(2, 20);

        // Assert
        pagedList.Items.Should().BeEmpty();
        pagedList.PageNumber.Should().Be(2);
        pagedList.PageSize.Should().Be(20);
        pagedList.TotalCount.Should().Be(0);
    }

    [Fact]
    public void Create_ShouldPaginateCorrectly()
    {
        // Arrange
        var allItems = Enumerable.Range(1, 50).Select(i => $"Item{i}").ToList();

        // Act
        var pagedList = PagedList<string>.Create(allItems, 2, 10);

        // Assert
        pagedList.Items.Should().HaveCount(10);
        pagedList.Items.First().Should().Be("Item11");
        pagedList.Items.Last().Should().Be("Item20");
        pagedList.PageNumber.Should().Be(2);
        pagedList.PageSize.Should().Be(10);
        pagedList.TotalCount.Should().Be(50);
        pagedList.TotalPages.Should().Be(5);
    }

    [Theory]
    [InlineData(1, 10, 100, 10)]
    [InlineData(2, 10, 100, 10)]
    [InlineData(10, 10, 100, 10)]
    public void TotalPages_WithDifferentScenarios_ShouldCalculateCorrectly(int pageNumber, int pageSize, int totalCount, int expectedTotalPages)
    {
        // Arrange
        var items = new List<string> { "Item1" };

        // Act
        var pagedList = new PagedList<string>(items, pageNumber, pageSize, totalCount);

        // Assert
        pagedList.TotalPages.Should().Be(expectedTotalPages);
    }

    [Fact]
    public void Items_ShouldBeReadOnly()
    {
        // Arrange
        var items = new List<string> { "Item1", "Item2" };
        var pagedList = new PagedList<string>(items, 1, 10, 100);

        // Act & Assert
        pagedList.Items.Should().BeAssignableTo<IReadOnlyList<string>>();
    }

    [Fact]
    public void Create_WithEmptyCollection_ShouldReturnEmptyPage()
    {
        // Arrange
        var emptyList = new List<string>();

        // Act
        var pagedList = PagedList<string>.Create(emptyList, 1, 10);

        // Assert
        pagedList.Items.Should().BeEmpty();
        pagedList.TotalCount.Should().Be(0);
        pagedList.TotalPages.Should().Be(0);
    }
}
