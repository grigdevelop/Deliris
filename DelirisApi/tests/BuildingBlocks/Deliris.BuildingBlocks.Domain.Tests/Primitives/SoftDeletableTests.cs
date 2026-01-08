using Deliris.BuildingBlocks.Domain.Tests.TestHelpers;

namespace Deliris.BuildingBlocks.Domain.Tests.Primitives;

[Trait("Category", "Unit")]
[Trait("Component", "SoftDeletable")]
public class SoftDeletableTests
{
    private readonly Faker _faker = new();

    [Fact]
    public void Delete_ShouldSetIsDeletedToTrue()
    {
        // Arrange
        var entity = new TestSoftDeletableEntity(Guid.NewGuid(), _faker.Commerce.ProductName());
        var deletedBy = _faker.Internet.UserName();

        // Act
        entity.Delete(deletedBy);

        // Assert
        entity.IsDeleted.Should().BeTrue();
    }

    [Fact]
    public void Delete_ShouldSetDeletedBy()
    {
        // Arrange
        var entity = new TestSoftDeletableEntity(Guid.NewGuid(), _faker.Commerce.ProductName());
        var deletedBy = _faker.Internet.UserName();

        // Act
        entity.Delete(deletedBy);

        // Assert
        entity.DeletedBy.Should().Be(deletedBy);
    }

    [Fact]
    public void Delete_ShouldSetDeletedAtUtc()
    {
        // Arrange
        var entity = new TestSoftDeletableEntity(Guid.NewGuid(), _faker.Commerce.ProductName());
        var deletedBy = _faker.Internet.UserName();
        var beforeDelete = DateTime.UtcNow;

        // Act
        entity.Delete(deletedBy);
        var afterDelete = DateTime.UtcNow;

        // Assert
        entity.DeletedAtUtc.Should().NotBeNull();
        entity.DeletedAtUtc.Should().BeOnOrAfter(beforeDelete);
        entity.DeletedAtUtc.Should().BeOnOrBefore(afterDelete);
    }

    [Fact]
    public void Delete_WithSpecificDateTime_ShouldSetDeletedAtUtc()
    {
        // Arrange
        var entity = new TestSoftDeletableEntity(Guid.NewGuid(), _faker.Commerce.ProductName());
        var deletedBy = _faker.Internet.UserName();
        var deletedAtUtc = DateTime.UtcNow.AddDays(-1);

        // Act
        entity.Delete(deletedBy, deletedAtUtc);

        // Assert
        entity.DeletedAtUtc.Should().Be(deletedAtUtc);
    }

    [Fact]
    public void Delete_WithNullDeletedBy_ShouldSetDeletedByToNull()
    {
        // Arrange
        var entity = new TestSoftDeletableEntity(Guid.NewGuid(), _faker.Commerce.ProductName());

        // Act
        entity.Delete(null);

        // Assert
        entity.DeletedBy.Should().BeNull();
        entity.IsDeleted.Should().BeTrue();
    }

    [Fact]
    public void Restore_ShouldSetIsDeletedToFalse()
    {
        // Arrange
        var entity = new TestSoftDeletableEntity(Guid.NewGuid(), _faker.Commerce.ProductName());
        entity.Delete(_faker.Internet.UserName());

        // Act
        entity.Restore();

        // Assert
        entity.IsDeleted.Should().BeFalse();
    }

    [Fact]
    public void Restore_ShouldClearDeletedBy()
    {
        // Arrange
        var entity = new TestSoftDeletableEntity(Guid.NewGuid(), _faker.Commerce.ProductName());
        entity.Delete(_faker.Internet.UserName());

        // Act
        entity.Restore();

        // Assert
        entity.DeletedBy.Should().BeNull();
    }

    [Fact]
    public void Restore_ShouldClearDeletedAtUtc()
    {
        // Arrange
        var entity = new TestSoftDeletableEntity(Guid.NewGuid(), _faker.Commerce.ProductName());
        entity.Delete(_faker.Internet.UserName());

        // Act
        entity.Restore();

        // Assert
        entity.DeletedAtUtc.Should().BeNull();
    }

    [Fact]
    public void ISoftDeletable_ShouldBeImplemented()
    {
        // Arrange
        var entity = new TestSoftDeletableEntity(Guid.NewGuid(), _faker.Commerce.ProductName());

        // Act & Assert
        entity.Should().BeAssignableTo<ISoftDeletable>();
    }

    [Fact]
    public void NewEntity_ShouldNotBeDeleted()
    {
        // Arrange & Act
        var entity = new TestSoftDeletableEntity(Guid.NewGuid(), _faker.Commerce.ProductName());

        // Assert
        entity.IsDeleted.Should().BeFalse();
        entity.DeletedBy.Should().BeNull();
        entity.DeletedAtUtc.Should().BeNull();
    }

    [Fact]
    public void DeleteAndRestore_ShouldWorkCorrectly()
    {
        // Arrange
        var entity = new TestSoftDeletableEntity(Guid.NewGuid(), _faker.Commerce.ProductName());
        var deletedBy = _faker.Internet.UserName();

        // Act
        entity.Delete(deletedBy);
        var wasDeleted = entity.IsDeleted;
        entity.Restore();

        // Assert
        wasDeleted.Should().BeTrue();
        entity.IsDeleted.Should().BeFalse();
        entity.DeletedBy.Should().BeNull();
        entity.DeletedAtUtc.Should().BeNull();
    }

    [Theory]
    [InlineData("user1")]
    [InlineData("admin")]
    [InlineData("system")]
    public void Delete_WithDifferentUsers_ShouldSetCorrectly(string deletedBy)
    {
        // Arrange
        var entity = new TestSoftDeletableEntity(Guid.NewGuid(), _faker.Commerce.ProductName());

        // Act
        entity.Delete(deletedBy);

        // Assert
        entity.DeletedBy.Should().Be(deletedBy);
        entity.IsDeleted.Should().BeTrue();
    }

    [Fact]
    public void MultipleDeletes_ShouldUpdateTimestamp()
    {
        // Arrange
        var entity = new TestSoftDeletableEntity(Guid.NewGuid(), _faker.Commerce.ProductName());
        entity.Delete("user1", DateTime.UtcNow.AddHours(-2));

        // Act
        var newDeleteTime = DateTime.UtcNow;
        entity.Delete("user2", newDeleteTime);

        // Assert
        entity.DeletedBy.Should().Be("user2");
        entity.DeletedAtUtc.Should().Be(newDeleteTime);
    }
}
