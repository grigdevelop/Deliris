using Deliris.BuildingBlocks.Domain.Tests.TestHelpers;

namespace Deliris.BuildingBlocks.Domain.Tests.Primitives;

[Trait("Category", "Unit")]
[Trait("Component", "AuditableEntity")]
public class AuditableEntityTests
{
    private readonly Faker _faker = new();

    [Fact]
    public void Constructor_ShouldInitializeCreatedAtUtc()
    {
        // Arrange
        var beforeCreation = DateTime.UtcNow;

        // Act
        var entity = new TestAuditableEntity(Guid.NewGuid(), _faker.Commerce.ProductName());
        var afterCreation = DateTime.UtcNow;

        // Assert
        entity.CreatedAtUtc.Should().BeOnOrAfter(beforeCreation);
        entity.CreatedAtUtc.Should().BeOnOrBefore(afterCreation);
        entity.CreatedBy.Should().BeNull();
        entity.UpdatedAtUtc.Should().BeNull();
        entity.UpdatedBy.Should().BeNull();
    }

    [Fact]
    public void SetCreatedAudit_ShouldSetCreatedByAndCreatedAtUtc()
    {
        // Arrange
        var entity = new TestAuditableEntity(Guid.NewGuid(), _faker.Commerce.ProductName());
        var createdBy = _faker.Internet.UserName();
        var createdAtUtc = DateTime.UtcNow.AddDays(-1);

        // Act
        entity.SetCreatedAudit(createdBy, createdAtUtc);

        // Assert
        entity.CreatedBy.Should().Be(createdBy);
        entity.CreatedAtUtc.Should().Be(createdAtUtc);
    }

    [Fact]
    public void SetCreatedAudit_WithoutDateTime_ShouldUseCurrentUtcTime()
    {
        // Arrange
        var entity = new TestAuditableEntity(Guid.NewGuid(), _faker.Commerce.ProductName());
        var createdBy = _faker.Internet.UserName();
        var beforeSet = DateTime.UtcNow;

        // Act
        entity.SetCreatedAudit(createdBy);
        var afterSet = DateTime.UtcNow;

        // Assert
        entity.CreatedBy.Should().Be(createdBy);
        entity.CreatedAtUtc.Should().BeOnOrAfter(beforeSet);
        entity.CreatedAtUtc.Should().BeOnOrBefore(afterSet);
    }

    [Fact]
    public void SetUpdatedAudit_ShouldSetUpdatedByAndUpdatedAtUtc()
    {
        // Arrange
        var entity = new TestAuditableEntity(Guid.NewGuid(), _faker.Commerce.ProductName());
        var updatedBy = _faker.Internet.UserName();
        var updatedAtUtc = DateTime.UtcNow;

        // Act
        entity.SetUpdatedAudit(updatedBy, updatedAtUtc);

        // Assert
        entity.UpdatedBy.Should().Be(updatedBy);
        entity.UpdatedAtUtc.Should().Be(updatedAtUtc);
    }

    [Fact]
    public void SetUpdatedAudit_WithoutDateTime_ShouldUseCurrentUtcTime()
    {
        // Arrange
        var entity = new TestAuditableEntity(Guid.NewGuid(), _faker.Commerce.ProductName());
        var updatedBy = _faker.Internet.UserName();
        var beforeSet = DateTime.UtcNow;

        // Act
        entity.SetUpdatedAudit(updatedBy);
        var afterSet = DateTime.UtcNow;

        // Assert
        entity.UpdatedBy.Should().Be(updatedBy);
        entity.UpdatedAtUtc.Should().NotBeNull();
        entity.UpdatedAtUtc.Should().BeOnOrAfter(beforeSet);
        entity.UpdatedAtUtc.Should().BeOnOrBefore(afterSet);
    }

    [Fact]
    public void SetCreatedAudit_WithNullCreatedBy_ShouldSetCreatedByToNull()
    {
        // Arrange
        var entity = new TestAuditableEntity(Guid.NewGuid(), _faker.Commerce.ProductName());

        // Act
        entity.SetCreatedAudit(null);

        // Assert
        entity.CreatedBy.Should().BeNull();
    }

    [Fact]
    public void SetUpdatedAudit_WithNullUpdatedBy_ShouldSetUpdatedByToNull()
    {
        // Arrange
        var entity = new TestAuditableEntity(Guid.NewGuid(), _faker.Commerce.ProductName());

        // Act
        entity.SetUpdatedAudit(null);

        // Assert
        entity.UpdatedBy.Should().BeNull();
        entity.UpdatedAtUtc.Should().NotBeNull();
    }

    [Fact]
    public void IAuditableEntity_ShouldBeImplemented()
    {
        // Arrange
        var entity = new TestAuditableEntity(Guid.NewGuid(), _faker.Commerce.ProductName());

        // Act & Assert
        entity.Should().BeAssignableTo<IAuditableEntity>();
    }

    [Fact]
    public void ParameterlessConstructor_ShouldCreateEntityForOrmCompatibility()
    {
        // Arrange & Act
        var entity = Activator.CreateInstance(typeof(TestAuditableEntity), nonPublic: true) as TestAuditableEntity;

        // Assert
        entity.Should().NotBeNull();
    }

    [Theory]
    [InlineData("user1")]
    [InlineData("admin")]
    [InlineData("system")]
    public void SetCreatedAudit_WithDifferentUsers_ShouldSetCorrectly(string createdBy)
    {
        // Arrange
        var entity = new TestAuditableEntity(Guid.NewGuid(), _faker.Commerce.ProductName());

        // Act
        entity.SetCreatedAudit(createdBy);

        // Assert
        entity.CreatedBy.Should().Be(createdBy);
    }

    [Fact]
    public void MultipleUpdates_ShouldUpdateTimestamp()
    {
        // Arrange
        var entity = new TestAuditableEntity(Guid.NewGuid(), _faker.Commerce.ProductName());
        entity.SetUpdatedAudit("user1", DateTime.UtcNow.AddHours(-2));

        // Act
        var newUpdateTime = DateTime.UtcNow;
        entity.SetUpdatedAudit("user2", newUpdateTime);

        // Assert
        entity.UpdatedBy.Should().Be("user2");
        entity.UpdatedAtUtc.Should().Be(newUpdateTime);
    }
}
