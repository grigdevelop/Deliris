using Deliris.BuildingBlocks.Domain.Tests.TestHelpers;

namespace Deliris.BuildingBlocks.Domain.Tests.Primitives;

[Trait("Category", "Unit")]
[Trait("Component", "Enumeration")]
public class EnumerationTests
{
    [Fact]
    public void GetAll_ShouldReturnAllEnumerationValues()
    {
        // Act
        var all = Enumeration.GetAll<TestEnumeration>();

        // Assert
        all.Should().HaveCount(3);
        all.Should().Contain(TestEnumeration.First);
        all.Should().Contain(TestEnumeration.Second);
        all.Should().Contain(TestEnumeration.Third);
    }

    [Fact]
    public void FromId_WithValidId_ShouldReturnCorrectEnumeration()
    {
        // Act
        var result = Enumeration.FromId<TestEnumeration>(1);

        // Assert
        result.Should().Be(TestEnumeration.First);
    }

    [Fact]
    public void FromId_WithInvalidId_ShouldThrowInvalidOperationException()
    {
        // Act
        Action act = () => Enumeration.FromId<TestEnumeration>(999);

        // Assert
        act.Should().Throw<InvalidOperationException>()
            .WithMessage("*is not a valid id in*");
    }

    [Fact]
    public void FromName_WithValidName_ShouldReturnCorrectEnumeration()
    {
        // Act
        var result = Enumeration.FromName<TestEnumeration>("Second");

        // Assert
        result.Should().Be(TestEnumeration.Second);
    }

    [Fact]
    public void FromName_WithInvalidName_ShouldThrowInvalidOperationException()
    {
        // Act
        Action act = () => Enumeration.FromName<TestEnumeration>("Invalid");

        // Assert
        act.Should().Throw<InvalidOperationException>()
            .WithMessage("*is not a valid name in*");
    }

    [Fact]
    public void TryFromId_WithValidId_ShouldReturnTrueAndValue()
    {
        // Act
        var success = Enumeration.TryFromId<TestEnumeration>(2, out var result);

        // Assert
        success.Should().BeTrue();
        result.Should().Be(TestEnumeration.Second);
    }

    [Fact]
    public void TryFromId_WithInvalidId_ShouldReturnFalseAndNull()
    {
        // Act
        var success = Enumeration.TryFromId<TestEnumeration>(999, out var result);

        // Assert
        success.Should().BeFalse();
        result.Should().BeNull();
    }

    [Fact]
    public void TryFromName_WithValidName_ShouldReturnTrueAndValue()
    {
        // Act
        var success = Enumeration.TryFromName<TestEnumeration>("Third", out var result);

        // Assert
        success.Should().BeTrue();
        result.Should().Be(TestEnumeration.Third);
    }

    [Fact]
    public void TryFromName_WithInvalidName_ShouldReturnFalseAndNull()
    {
        // Act
        var success = Enumeration.TryFromName<TestEnumeration>("Invalid", out var result);

        // Assert
        success.Should().BeFalse();
        result.Should().BeNull();
    }

    [Fact]
    public void Equals_WithSameIdAndType_ShouldReturnTrue()
    {
        // Arrange
        var enum1 = TestEnumeration.First;
        var enum2 = Enumeration.FromId<TestEnumeration>(1);

        // Act
        var result = enum1.Equals(enum2);

        // Assert
        result.Should().BeTrue();
    }

    [Fact]
    public void Equals_WithDifferentId_ShouldReturnFalse()
    {
        // Arrange
        var enum1 = TestEnumeration.First;
        var enum2 = TestEnumeration.Second;

        // Act
        var result = enum1.Equals(enum2);

        // Assert
        result.Should().BeFalse();
    }

    [Fact]
    public void Equals_WithNull_ShouldReturnFalse()
    {
        // Arrange
        var enumeration = TestEnumeration.First;

        // Act
        var result = enumeration.Equals(null);

        // Assert
        result.Should().BeFalse();
    }

    [Fact]
    public void GetHashCode_WithSameId_ShouldReturnSameHashCode()
    {
        // Arrange
        var enum1 = TestEnumeration.First;
        var enum2 = Enumeration.FromId<TestEnumeration>(1);

        // Act
        var hash1 = enum1.GetHashCode();
        var hash2 = enum2.GetHashCode();

        // Assert
        hash1.Should().Be(hash2);
    }

    [Fact]
    public void ToString_ShouldReturnName()
    {
        // Arrange
        var enumeration = TestEnumeration.First;

        // Act
        var result = enumeration.ToString();

        // Assert
        result.Should().Be("First");
    }

    [Fact]
    public void CompareTo_WithSmallerValue_ShouldReturnPositive()
    {
        // Arrange
        var enum1 = TestEnumeration.Second;
        var enum2 = TestEnumeration.First;

        // Act
        var result = enum1.CompareTo(enum2);

        // Assert
        result.Should().BeGreaterThan(0);
    }

    [Fact]
    public void CompareTo_WithLargerValue_ShouldReturnNegative()
    {
        // Arrange
        var enum1 = TestEnumeration.First;
        var enum2 = TestEnumeration.Third;

        // Act
        var result = enum1.CompareTo(enum2);

        // Assert
        result.Should().BeLessThan(0);
    }

    [Fact]
    public void CompareTo_WithSameValue_ShouldReturnZero()
    {
        // Arrange
        var enum1 = TestEnumeration.Second;
        var enum2 = Enumeration.FromId<TestEnumeration>(2);

        // Act
        var result = enum1.CompareTo(enum2);

        // Assert
        result.Should().Be(0);
    }

    [Fact]
    public void CompareTo_WithNull_ShouldReturnPositive()
    {
        // Arrange
        var enumeration = TestEnumeration.First;

        // Act
        var result = enumeration.CompareTo(null);

        // Assert
        result.Should().BeGreaterThan(0);
    }

    [Theory]
    [InlineData(1, "First")]
    [InlineData(2, "Second")]
    [InlineData(3, "Third")]
    public void FromId_ShouldMatchFromName(int id, string name)
    {
        // Act
        var fromId = Enumeration.FromId<TestEnumeration>(id);
        var fromName = Enumeration.FromName<TestEnumeration>(name);

        // Assert
        fromId.Should().Be(fromName);
    }
}
