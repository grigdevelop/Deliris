using Deliris.BuildingBlocks.Domain.Tests.TestHelpers;

namespace Deliris.BuildingBlocks.Domain.Tests.Primitives;

[Trait("Category", "Unit")]
[Trait("Component", "DomainEvent")]
public class DomainEventTests
{
    private readonly Faker _faker = new();

    [Fact]
    public void Constructor_ShouldInitializeIdAndOccurredOnUtc()
    {
        // Arrange
        var beforeCreation = DateTime.UtcNow;

        // Act
        var domainEvent = new TestDomainEvent(Guid.NewGuid(), _faker.Commerce.ProductName());
        var afterCreation = DateTime.UtcNow;

        // Assert
        domainEvent.Id.Should().NotBeEmpty();
        domainEvent.OccurredOnUtc.Should().BeOnOrAfter(beforeCreation);
        domainEvent.OccurredOnUtc.Should().BeOnOrBefore(afterCreation);
    }

    [Fact]
    public void Constructor_ShouldGenerateUniqueIds()
    {
        // Arrange & Act
        var event1 = new TestDomainEvent(Guid.NewGuid(), "Event1");
        var event2 = new TestDomainEvent(Guid.NewGuid(), "Event2");

        // Assert
        event1.Id.Should().NotBe(event2.Id);
    }

    [Fact]
    public void DomainEvent_ShouldBeImmutable()
    {
        // Arrange
        var entityId = Guid.NewGuid();
        var name = _faker.Commerce.ProductName();

        // Act
        var domainEvent = new TestDomainEvent(entityId, name);

        // Assert
        domainEvent.EntityId.Should().Be(entityId);
        domainEvent.Name.Should().Be(name);
        
        typeof(TestDomainEvent).GetProperty(nameof(TestDomainEvent.EntityId))!.CanWrite.Should().BeFalse();
        typeof(TestDomainEvent).GetProperty(nameof(TestDomainEvent.Name))!.CanWrite.Should().BeFalse();
    }

    [Fact]
    public void DomainEvent_ShouldImplementIDomainEvent()
    {
        // Arrange
        var domainEvent = new TestDomainEvent(Guid.NewGuid(), _faker.Commerce.ProductName());

        // Act & Assert
        domainEvent.Should().BeAssignableTo<IDomainEvent>();
    }

    [Fact]
    public void DomainEvent_AsRecord_ShouldSupportValueEquality()
    {
        // Arrange
        var entityId = Guid.NewGuid();
        var name = "TestName";

        // Act
        var event1 = new TestDomainEvent(entityId, name);
        var event2 = new TestDomainEvent(entityId, name);

        // Assert - Records with same values should be equal
        event1.EntityId.Should().Be(event2.EntityId);
        event1.Name.Should().Be(event2.Name);
        // Note: Id and OccurredOnUtc will be different, so the records won't be equal
        event1.Should().NotBe(event2);
    }

    [Fact]
    public void DomainEvent_ShouldHaveUniqueIdPerInstance()
    {
        // Arrange
        var entityId = Guid.NewGuid();
        var name = "TestName";

        // Act
        var event1 = new TestDomainEvent(entityId, name);
        var event2 = new TestDomainEvent(entityId, name);

        // Assert
        event1.Id.Should().NotBe(event2.Id);
    }

    [Fact]
    public void OccurredOnUtc_ShouldBeInUtc()
    {
        // Arrange & Act
        var domainEvent = new TestDomainEvent(Guid.NewGuid(), _faker.Commerce.ProductName());

        // Assert
        domainEvent.OccurredOnUtc.Kind.Should().Be(DateTimeKind.Utc);
    }

    [Theory]
    [InlineData("Event1")]
    [InlineData("Event2")]
    [InlineData("Event3")]
    public void Constructor_WithDifferentNames_ShouldCreateCorrectly(string name)
    {
        // Arrange
        var entityId = Guid.NewGuid();

        // Act
        var domainEvent = new TestDomainEvent(entityId, name);

        // Assert
        domainEvent.EntityId.Should().Be(entityId);
        domainEvent.Name.Should().Be(name);
    }

    [Fact]
    public void DomainEvent_MultipleInstances_ShouldHaveIncreasingTimestamps()
    {
        // Arrange & Act
        var event1 = new TestDomainEvent(Guid.NewGuid(), "Event1");
        Thread.Sleep(10); // Small delay to ensure different timestamps
        var event2 = new TestDomainEvent(Guid.NewGuid(), "Event2");

        // Assert
        event2.OccurredOnUtc.Should().BeOnOrAfter(event1.OccurredOnUtc);
    }
}
