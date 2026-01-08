using Deliris.BuildingBlocks.Domain.Tests.TestHelpers;

namespace Deliris.BuildingBlocks.Domain.Tests.Primitives;

[Trait("Category", "Unit")]
[Trait("Component", "AuditableAggregateRoot")]
public class AuditableAggregateRootTests
{
    private readonly Faker _faker = new();

    [Fact]
    public void Constructor_ShouldInitializeCreatedAtUtcAndDomainEvents()
    {
        // Arrange
        var beforeCreation = DateTime.UtcNow;

        // Act
        var aggregate = new TestAuditableAggregateRoot(Guid.NewGuid(), _faker.Commerce.ProductName());
        var afterCreation = DateTime.UtcNow;

        // Assert
        aggregate.CreatedAtUtc.Should().BeOnOrAfter(beforeCreation);
        aggregate.CreatedAtUtc.Should().BeOnOrBefore(afterCreation);
        aggregate.DomainEvents.Should().BeEmpty();
    }

    [Fact]
    public void SetCreatedAudit_ShouldSetAuditFields()
    {
        // Arrange
        var aggregate = new TestAuditableAggregateRoot(Guid.NewGuid(), _faker.Commerce.ProductName());
        var createdBy = _faker.Internet.UserName();

        // Act
        aggregate.SetCreatedAudit(createdBy);

        // Assert
        aggregate.CreatedBy.Should().Be(createdBy);
    }

    [Fact]
    public void SetUpdatedAudit_ShouldSetAuditFields()
    {
        // Arrange
        var aggregate = new TestAuditableAggregateRoot(Guid.NewGuid(), _faker.Commerce.ProductName());
        var updatedBy = _faker.Internet.UserName();

        // Act
        aggregate.SetUpdatedAudit(updatedBy);

        // Assert
        aggregate.UpdatedBy.Should().Be(updatedBy);
        aggregate.UpdatedAtUtc.Should().NotBeNull();
    }

    [Fact]
    public void ChangeName_ShouldRaiseDomainEventAndMaintainAudit()
    {
        // Arrange
        var aggregate = new TestAuditableAggregateRoot(Guid.NewGuid(), _faker.Commerce.ProductName());
        var newName = _faker.Commerce.ProductName();

        // Act
        aggregate.ChangeName(newName);

        // Assert
        aggregate.Name.Should().Be(newName);
        aggregate.DomainEvents.Should().HaveCount(1);
    }

    [Fact]
    public void IAuditableEntity_ShouldBeImplemented()
    {
        // Arrange
        var aggregate = new TestAuditableAggregateRoot(Guid.NewGuid(), _faker.Commerce.ProductName());

        // Act & Assert
        aggregate.Should().BeAssignableTo<IAuditableEntity>();
    }

    [Fact]
    public void AggregateRoot_ShouldBeInherited()
    {
        // Arrange
        var aggregate = new TestAuditableAggregateRoot(Guid.NewGuid(), _faker.Commerce.ProductName());

        // Act & Assert
        aggregate.Should().BeAssignableTo<AggregateRoot<Guid>>();
    }

    [Fact]
    public void DomainEventsAndAudit_ShouldWorkTogether()
    {
        // Arrange
        var aggregate = new TestAuditableAggregateRoot(Guid.NewGuid(), _faker.Commerce.ProductName());
        aggregate.SetCreatedAudit("creator");

        // Act
        aggregate.ChangeName("NewName");
        aggregate.SetUpdatedAudit("updater");

        // Assert
        aggregate.CreatedBy.Should().Be("creator");
        aggregate.UpdatedBy.Should().Be("updater");
        aggregate.DomainEvents.Should().HaveCount(1);
    }

    [Fact]
    public void ClearDomainEvents_ShouldNotAffectAuditFields()
    {
        // Arrange
        var aggregate = new TestAuditableAggregateRoot(Guid.NewGuid(), _faker.Commerce.ProductName());
        aggregate.SetCreatedAudit("creator");
        aggregate.ChangeName("NewName");

        // Act
        aggregate.ClearDomainEvents();

        // Assert
        aggregate.DomainEvents.Should().BeEmpty();
        aggregate.CreatedBy.Should().Be("creator");
    }
}
