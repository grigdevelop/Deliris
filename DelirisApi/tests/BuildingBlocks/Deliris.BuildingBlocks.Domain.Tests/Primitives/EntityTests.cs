using Deliris.BuildingBlocks.Domain.Tests.TestHelpers;

namespace Deliris.BuildingBlocks.Domain.Tests.Primitives;

[Trait("Category", "Unit")]
[Trait("Component", "Entity")]
public class EntityTests
{
    private readonly Faker _faker = new();

    [Fact]
    public void Constructor_WithValidId_ShouldCreateEntity()
    {
        // Arrange
        var id = Guid.NewGuid();
        var name = _faker.Commerce.ProductName();

        // Act
        var entity = new TestEntity(id, name);

        // Assert
        entity.Should().NotBeNull();
        entity.Id.Should().Be(id);
        entity.Name.Should().Be(name);
    }

    [Fact]
    public void ParameterlessConstructor_ShouldCreateEntityForOrmCompatibility()
    {
        // Arrange & Act
        var entity = Activator.CreateInstance(typeof(TestEntity), nonPublic: true) as TestEntity;

        // Assert
        entity.Should().NotBeNull();
    }

    [Fact]
    public void Equals_WithSameId_ShouldReturnTrue()
    {
        // Arrange
        var id = Guid.NewGuid();
        var entity1 = new TestEntity(id, _faker.Commerce.ProductName());
        var entity2 = new TestEntity(id, _faker.Commerce.ProductName());

        // Act
        var result = entity1.Equals(entity2);

        // Assert
        result.Should().BeTrue();
    }

    [Fact]
    public void Equals_WithDifferentId_ShouldReturnFalse()
    {
        // Arrange
        var entity1 = new TestEntity(Guid.NewGuid(), _faker.Commerce.ProductName());
        var entity2 = new TestEntity(Guid.NewGuid(), _faker.Commerce.ProductName());

        // Act
        var result = entity1.Equals(entity2);

        // Assert
        result.Should().BeFalse();
    }

    [Fact]
    public void Equals_WithNull_ShouldReturnFalse()
    {
        // Arrange
        var entity = new TestEntity(Guid.NewGuid(), _faker.Commerce.ProductName());

        // Act
        var result = entity.Equals(null);

        // Assert
        result.Should().BeFalse();
    }

    [Fact]
    public void Equals_WithSameReference_ShouldReturnTrue()
    {
        // Arrange
        var entity = new TestEntity(Guid.NewGuid(), _faker.Commerce.ProductName());

        // Act
        var result = entity.Equals(entity);

        // Assert
        result.Should().BeTrue();
    }

    [Fact]
    public void Equals_WithDifferentType_ShouldReturnFalse()
    {
        // Arrange
        var entity = new TestEntity(Guid.NewGuid(), _faker.Commerce.ProductName());
        var differentObject = new object();

        // Act
        var result = entity.Equals(differentObject);

        // Assert
        result.Should().BeFalse();
    }

    [Fact]
    public void GetHashCode_WithSameId_ShouldReturnSameHashCode()
    {
        // Arrange
        var id = Guid.NewGuid();
        var entity1 = new TestEntity(id, _faker.Commerce.ProductName());
        var entity2 = new TestEntity(id, _faker.Commerce.ProductName());

        // Act
        var hashCode1 = entity1.GetHashCode();
        var hashCode2 = entity2.GetHashCode();

        // Assert
        hashCode1.Should().Be(hashCode2);
    }

    [Fact]
    public void GetHashCode_WithDifferentId_ShouldReturnDifferentHashCode()
    {
        // Arrange
        var entity1 = new TestEntity(Guid.NewGuid(), _faker.Commerce.ProductName());
        var entity2 = new TestEntity(Guid.NewGuid(), _faker.Commerce.ProductName());

        // Act
        var hashCode1 = entity1.GetHashCode();
        var hashCode2 = entity2.GetHashCode();

        // Assert
        hashCode1.Should().NotBe(hashCode2);
    }

    [Fact]
    public void EqualityOperator_WithSameId_ShouldReturnTrue()
    {
        // Arrange
        var id = Guid.NewGuid();
        var entity1 = new TestEntity(id, _faker.Commerce.ProductName());
        var entity2 = new TestEntity(id, _faker.Commerce.ProductName());

        // Act
        var result = entity1 == entity2;

        // Assert
        result.Should().BeTrue();
    }

    [Fact]
    public void EqualityOperator_WithDifferentId_ShouldReturnFalse()
    {
        // Arrange
        var entity1 = new TestEntity(Guid.NewGuid(), _faker.Commerce.ProductName());
        var entity2 = new TestEntity(Guid.NewGuid(), _faker.Commerce.ProductName());

        // Act
        var result = entity1 == entity2;

        // Assert
        result.Should().BeFalse();
    }

    [Fact]
    public void EqualityOperator_WithBothNull_ShouldReturnTrue()
    {
        // Arrange
        TestEntity? entity1 = null;
        TestEntity? entity2 = null;

        // Act
        var result = entity1 == entity2;

        // Assert
        result.Should().BeTrue();
    }

    [Fact]
    public void EqualityOperator_WithOneNull_ShouldReturnFalse()
    {
        // Arrange
        var entity1 = new TestEntity(Guid.NewGuid(), _faker.Commerce.ProductName());
        TestEntity? entity2 = null;

        // Act
        var result = entity1 == entity2;

        // Assert
        result.Should().BeFalse();
    }

    [Fact]
    public void InequalityOperator_WithSameId_ShouldReturnFalse()
    {
        // Arrange
        var id = Guid.NewGuid();
        var entity1 = new TestEntity(id, _faker.Commerce.ProductName());
        var entity2 = new TestEntity(id, _faker.Commerce.ProductName());

        // Act
        var result = entity1 != entity2;

        // Assert
        result.Should().BeFalse();
    }

    [Fact]
    public void InequalityOperator_WithDifferentId_ShouldReturnTrue()
    {
        // Arrange
        var entity1 = new TestEntity(Guid.NewGuid(), _faker.Commerce.ProductName());
        var entity2 = new TestEntity(Guid.NewGuid(), _faker.Commerce.ProductName());

        // Act
        var result = entity1 != entity2;

        // Assert
        result.Should().BeTrue();
    }

    [Theory]
    [InlineData("Product A", "Product B")]
    [InlineData("Product X", "Product Y")]
    public void Equals_WithSameIdButDifferentProperties_ShouldReturnTrue(string name1, string name2)
    {
        // Arrange
        var id = Guid.NewGuid();
        var entity1 = new TestEntity(id, name1);
        var entity2 = new TestEntity(id, name2);

        // Act
        var result = entity1.Equals(entity2);

        // Assert
        result.Should().BeTrue("entities with the same ID should be equal regardless of other properties");
    }
}
