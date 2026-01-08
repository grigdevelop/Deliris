using Deliris.BuildingBlocks.Domain.Specifications;
using Deliris.BuildingBlocks.Domain.Tests.TestHelpers;

namespace Deliris.BuildingBlocks.Domain.Tests.Specifications;

[Trait("Category", "Unit")]
[Trait("Component", "Specification")]
public class SpecificationTests
{
    private readonly Faker _faker = new();

    [Fact]
    public void Constructor_WithCriteria_ShouldSetCriteria()
    {
        // Arrange
        var nameFilter = _faker.Commerce.ProductName();

        // Act
        var specification = new TestSpecification(nameFilter);

        // Assert
        specification.Criteria.Should().NotBeNull();
    }

    [Fact]
    public void AddCriteria_ShouldSetCriteria()
    {
        // Arrange
        var specification = new TestSpecification();

        // Act
        specification.AddTestCriteria(e => e.Name == "Test");

        // Assert
        specification.Criteria.Should().NotBeNull();
    }

    [Fact]
    public void AddInclude_WithExpression_ShouldAddToIncludesList()
    {
        // Arrange
        var specification = new TestSpecification();

        // Act
        specification.AddTestInclude(e => e.Name);

        // Assert
        specification.Includes.Should().HaveCount(1);
    }

    [Fact]
    public void AddInclude_WithString_ShouldAddToIncludeStringsList()
    {
        // Arrange
        var specification = new TestSpecification();

        // Act
        specification.AddTestInclude("Name");

        // Assert
        specification.IncludeStrings.Should().HaveCount(1);
        specification.IncludeStrings.Should().Contain("Name");
    }

    [Fact]
    public void AddOrderBy_ShouldSetOrderByExpression()
    {
        // Arrange
        var specification = new TestSpecification();

        // Act
        specification.AddTestOrderBy(e => e.Name);

        // Assert
        specification.OrderBy.Should().NotBeNull();
    }

    [Fact]
    public void AddOrderByDescending_ShouldSetOrderByDescendingExpression()
    {
        // Arrange
        var specification = new TestSpecification();

        // Act
        specification.AddTestOrderByDescending(e => e.Name);

        // Assert
        specification.OrderByDescending.Should().NotBeNull();
    }

    [Fact]
    public void AddGroupBy_ShouldSetGroupByExpression()
    {
        // Arrange
        var specification = new TestSpecification();

        // Act
        specification.AddTestGroupBy(e => e.Name);

        // Assert
        specification.GroupBy.Should().NotBeNull();
    }

    [Fact]
    public void ApplyPaging_ShouldSetSkipAndTake()
    {
        // Arrange
        var specification = new TestSpecification();
        var skip = 10;
        var take = 20;

        // Act
        specification.ApplyTestPaging(skip, take);

        // Assert
        specification.Skip.Should().Be(skip);
        specification.Take.Should().Be(take);
    }

    [Fact]
    public void AsNoTracking_ShouldDisableTracking()
    {
        // Arrange
        var specification = new TestSpecification();

        // Act
        specification.SetNoTracking();

        // Assert
        specification.IsTrackingEnabled.Should().BeFalse();
    }

    [Fact]
    public void AsSplitQuery_ShouldEnableSplitQuery()
    {
        // Arrange
        var specification = new TestSpecification();

        // Act
        specification.SetSplitQuery();

        // Assert
        specification.IsSplitQuery.Should().BeTrue();
    }

    [Fact]
    public void DefaultSpecification_ShouldHaveTrackingEnabled()
    {
        // Arrange & Act
        var specification = new TestSpecification();

        // Assert
        specification.IsTrackingEnabled.Should().BeTrue();
    }

    [Fact]
    public void DefaultSpecification_ShouldNotHaveSplitQuery()
    {
        // Arrange & Act
        var specification = new TestSpecification();

        // Assert
        specification.IsSplitQuery.Should().BeFalse();
    }

    [Fact]
    public void And_ShouldCombineSpecifications()
    {
        // Arrange
        var spec1 = new TestSpecification();
        spec1.AddTestCriteria(e => e.Name.StartsWith("A"));

        var spec2 = new TestSpecification();
        spec2.AddTestCriteria(e => e.Name.EndsWith("Z"));

        // Act
        var combinedSpec = spec1.And(spec2);

        // Assert
        combinedSpec.Should().NotBeNull();
        combinedSpec.Criteria.Should().NotBeNull();
    }

    [Fact]
    public void Or_ShouldCombineSpecifications()
    {
        // Arrange
        var spec1 = new TestSpecification();
        spec1.AddTestCriteria(e => e.Name == "A");

        var spec2 = new TestSpecification();
        spec2.AddTestCriteria(e => e.Name == "B");

        // Act
        var combinedSpec = spec1.Or(spec2);

        // Assert
        combinedSpec.Should().NotBeNull();
        combinedSpec.Criteria.Should().NotBeNull();
    }

    [Fact]
    public void Not_ShouldNegateSpecification()
    {
        // Arrange
        var spec = new TestSpecification();
        spec.AddTestCriteria(e => e.Name == "Test");

        // Act
        var negatedSpec = spec.Not();

        // Assert
        negatedSpec.Should().NotBeNull();
        negatedSpec.Criteria.Should().NotBeNull();
    }

    [Theory]
    [InlineData(0, 10)]
    [InlineData(10, 20)]
    [InlineData(20, 50)]
    public void ApplyPaging_WithDifferentValues_ShouldSetCorrectly(int skip, int take)
    {
        // Arrange
        var specification = new TestSpecification();

        // Act
        specification.ApplyTestPaging(skip, take);

        // Assert
        specification.Skip.Should().Be(skip);
        specification.Take.Should().Be(take);
    }

    [Fact]
    public void MultipleIncludes_ShouldAccumulate()
    {
        // Arrange
        var specification = new TestSpecification();

        // Act
        specification.AddTestInclude(e => e.Name);
        specification.AddTestInclude(e => e.Id);

        // Assert
        specification.Includes.Should().HaveCount(2);
    }

    [Fact]
    public void MultipleIncludeStrings_ShouldAccumulate()
    {
        // Arrange
        var specification = new TestSpecification();

        // Act
        specification.AddTestInclude("Name");
        specification.AddTestInclude("Id");

        // Assert
        specification.IncludeStrings.Should().HaveCount(2);
    }
}
