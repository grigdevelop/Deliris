using Deliris.BuildingBlocks.Domain.Tests.TestHelpers;

namespace Deliris.BuildingBlocks.Domain.Tests.Primitives;

[Trait("Category", "Unit")]
[Trait("Component", "ValueObject")]
public class ValueObjectTests
{
    private readonly Faker _faker = new();

    [Fact]
    public void Equals_WithSameValues_ShouldReturnTrue()
    {
        // Arrange
        var street = _faker.Address.StreetAddress();
        var city = _faker.Address.City();
        var zipCode = _faker.Address.ZipCode();

        var valueObject1 = new TestValueObject(street, city, zipCode);
        var valueObject2 = new TestValueObject(street, city, zipCode);

        // Act
        var result = valueObject1.Equals(valueObject2);

        // Assert
        result.Should().BeTrue();
    }

    [Fact]
    public void Equals_WithDifferentValues_ShouldReturnFalse()
    {
        // Arrange
        var valueObject1 = new TestValueObject(
            _faker.Address.StreetAddress(),
            _faker.Address.City(),
            _faker.Address.ZipCode());

        var valueObject2 = new TestValueObject(
            _faker.Address.StreetAddress(),
            _faker.Address.City(),
            _faker.Address.ZipCode());

        // Act
        var result = valueObject1.Equals(valueObject2);

        // Assert
        result.Should().BeFalse();
    }

    [Fact]
    public void Equals_WithNull_ShouldReturnFalse()
    {
        // Arrange
        var valueObject = new TestValueObject(
            _faker.Address.StreetAddress(),
            _faker.Address.City(),
            _faker.Address.ZipCode());

        // Act
        var result = valueObject.Equals(null);

        // Assert
        result.Should().BeFalse();
    }

    [Fact]
    public void Equals_WithSameReference_ShouldReturnTrue()
    {
        // Arrange
        var valueObject = new TestValueObject(
            _faker.Address.StreetAddress(),
            _faker.Address.City(),
            _faker.Address.ZipCode());

        // Act
        var result = valueObject.Equals(valueObject);

        // Assert
        result.Should().BeTrue();
    }

    [Fact]
    public void Equals_WithDifferentType_ShouldReturnFalse()
    {
        // Arrange
        var valueObject = new TestValueObject(
            _faker.Address.StreetAddress(),
            _faker.Address.City(),
            _faker.Address.ZipCode());
        var differentObject = new object();

        // Act
        var result = valueObject.Equals(differentObject);

        // Assert
        result.Should().BeFalse();
    }

    [Fact]
    public void GetHashCode_WithSameValues_ShouldReturnSameHashCode()
    {
        // Arrange
        var street = _faker.Address.StreetAddress();
        var city = _faker.Address.City();
        var zipCode = _faker.Address.ZipCode();

        var valueObject1 = new TestValueObject(street, city, zipCode);
        var valueObject2 = new TestValueObject(street, city, zipCode);

        // Act
        var hashCode1 = valueObject1.GetHashCode();
        var hashCode2 = valueObject2.GetHashCode();

        // Assert
        hashCode1.Should().Be(hashCode2);
    }

    [Fact]
    public void GetHashCode_WithDifferentValues_ShouldReturnDifferentHashCode()
    {
        // Arrange
        var valueObject1 = new TestValueObject(
            _faker.Address.StreetAddress(),
            _faker.Address.City(),
            _faker.Address.ZipCode());

        var valueObject2 = new TestValueObject(
            _faker.Address.StreetAddress(),
            _faker.Address.City(),
            _faker.Address.ZipCode());

        // Act
        var hashCode1 = valueObject1.GetHashCode();
        var hashCode2 = valueObject2.GetHashCode();

        // Assert
        hashCode1.Should().NotBe(hashCode2);
    }

    [Fact]
    public void EqualityOperator_WithSameValues_ShouldReturnTrue()
    {
        // Arrange
        var street = _faker.Address.StreetAddress();
        var city = _faker.Address.City();
        var zipCode = _faker.Address.ZipCode();

        var valueObject1 = new TestValueObject(street, city, zipCode);
        var valueObject2 = new TestValueObject(street, city, zipCode);

        // Act
        var result = valueObject1 == valueObject2;

        // Assert
        result.Should().BeTrue();
    }

    [Fact]
    public void EqualityOperator_WithDifferentValues_ShouldReturnFalse()
    {
        // Arrange
        var valueObject1 = new TestValueObject(
            _faker.Address.StreetAddress(),
            _faker.Address.City(),
            _faker.Address.ZipCode());

        var valueObject2 = new TestValueObject(
            _faker.Address.StreetAddress(),
            _faker.Address.City(),
            _faker.Address.ZipCode());

        // Act
        var result = valueObject1 == valueObject2;

        // Assert
        result.Should().BeFalse();
    }

    [Fact]
    public void EqualityOperator_WithBothNull_ShouldReturnTrue()
    {
        // Arrange
        TestValueObject? valueObject1 = null;
        TestValueObject? valueObject2 = null;

        // Act
        var result = valueObject1 == valueObject2;

        // Assert
        result.Should().BeTrue();
    }

    [Fact]
    public void InequalityOperator_WithSameValues_ShouldReturnFalse()
    {
        // Arrange
        var street = _faker.Address.StreetAddress();
        var city = _faker.Address.City();
        var zipCode = _faker.Address.ZipCode();

        var valueObject1 = new TestValueObject(street, city, zipCode);
        var valueObject2 = new TestValueObject(street, city, zipCode);

        // Act
        var result = valueObject1 != valueObject2;

        // Assert
        result.Should().BeFalse();
    }

    [Fact]
    public void InequalityOperator_WithDifferentValues_ShouldReturnTrue()
    {
        // Arrange
        var valueObject1 = new TestValueObject(
            _faker.Address.StreetAddress(),
            _faker.Address.City(),
            _faker.Address.ZipCode());

        var valueObject2 = new TestValueObject(
            _faker.Address.StreetAddress(),
            _faker.Address.City(),
            _faker.Address.ZipCode());

        // Act
        var result = valueObject1 != valueObject2;

        // Assert
        result.Should().BeTrue();
    }

    [Fact]
    public void ValueObject_ShouldBeImmutable()
    {
        // Arrange
        var street = _faker.Address.StreetAddress();
        var city = _faker.Address.City();
        var zipCode = _faker.Address.ZipCode();

        var valueObject = new TestValueObject(street, city, zipCode);

        // Act & Assert
        valueObject.Street.Should().Be(street);
        valueObject.City.Should().Be(city);
        valueObject.ZipCode.Should().Be(zipCode);

        typeof(TestValueObject).GetProperty(nameof(TestValueObject.Street))!.CanWrite.Should().BeFalse();
        typeof(TestValueObject).GetProperty(nameof(TestValueObject.City))!.CanWrite.Should().BeFalse();
        typeof(TestValueObject).GetProperty(nameof(TestValueObject.ZipCode))!.CanWrite.Should().BeFalse();
    }

    [Theory]
    [InlineData("123 Main St", "New York", "10001")]
    [InlineData("456 Oak Ave", "Los Angeles", "90001")]
    [InlineData("789 Pine Rd", "Chicago", "60601")]
    public void Equals_WithIdenticalValues_ShouldAlwaysReturnTrue(string street, string city, string zipCode)
    {
        // Arrange
        var valueObject1 = new TestValueObject(street, city, zipCode);
        var valueObject2 = new TestValueObject(street, city, zipCode);

        // Act
        var result = valueObject1.Equals(valueObject2);

        // Assert
        result.Should().BeTrue();
    }
}
