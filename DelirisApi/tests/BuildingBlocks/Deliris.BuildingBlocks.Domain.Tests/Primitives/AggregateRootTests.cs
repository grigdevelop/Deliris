using Deliris.BuildingBlocks.Domain.Tests.TestHelpers;

namespace Deliris.BuildingBlocks.Domain.Tests.Primitives;

[Trait("Category", "Unit")]
[Trait("Component", "AggregateRoot")]
public class AggregateRootTests
{
    private readonly Faker _faker = new();

    [Fact]
    public void Constructor_WithValidId_ShouldCreateAggregateRoot()
    {
        // Arrange
        var id = Guid.NewGuid();
        var name = _faker.Commerce.ProductName();

        // Act
        var aggregate = new TestAggregateRoot(id, name);

        // Assert
        aggregate.Should().NotBeNull();
        aggregate.Id.Should().Be(id);
        aggregate.Name.Should().Be(name);
        aggregate.DomainEvents.Should().BeEmpty();
    }

    [Fact]
    public void RaiseDomainEvent_ShouldAddEventToCollection()
    {
        // Arrange
        var aggregate = new TestAggregateRoot(Guid.NewGuid(), _faker.Commerce.ProductName());
        var domainEvent = new TestDomainEvent(aggregate.Id, "Test");

        // Act
        aggregate.RaiseTestEvent(domainEvent);

        // Assert
        aggregate.DomainEvents.Should().HaveCount(1);
        aggregate.DomainEvents.Should().Contain(domainEvent);
    }

    [Fact]
    public void RaiseDomainEvent_WithMultipleEvents_ShouldMaintainOrder()
    {
        // Arrange
        var aggregate = new TestAggregateRoot(Guid.NewGuid(), _faker.Commerce.ProductName());
        var event1 = new TestDomainEvent(aggregate.Id, "Event1");
        var event2 = new TestDomainEvent(aggregate.Id, "Event2");
        var event3 = new TestDomainEvent(aggregate.Id, "Event3");

        // Act
        aggregate.RaiseTestEvent(event1);
        aggregate.RaiseTestEvent(event2);
        aggregate.RaiseTestEvent(event3);

        // Assert
        aggregate.DomainEvents.Should().HaveCount(3);
        aggregate.DomainEvents.Should().ContainInOrder(event1, event2, event3);
    }

    [Fact]
    public void RaiseDomainEvent_WithNullEvent_ShouldThrowArgumentNullException()
    {
        // Arrange
        var aggregate = new TestAggregateRoot(Guid.NewGuid(), _faker.Commerce.ProductName());

        // Act
        Action act = () => aggregate.RaiseTestEvent(null!);

        // Assert
        act.Should().Throw<ArgumentNullException>();
    }

    [Fact]
    public void ClearDomainEvents_ShouldRemoveAllEvents()
    {
        // Arrange
        var aggregate = new TestAggregateRoot(Guid.NewGuid(), _faker.Commerce.ProductName());
        aggregate.RaiseTestEvent(new TestDomainEvent(aggregate.Id, "Event1"));
        aggregate.RaiseTestEvent(new TestDomainEvent(aggregate.Id, "Event2"));

        // Act
        aggregate.ClearDomainEvents();

        // Assert
        aggregate.DomainEvents.Should().BeEmpty();
    }

    [Fact]
    public void DomainEvents_ShouldBeReadOnly()
    {
        // Arrange
        var aggregate = new TestAggregateRoot(Guid.NewGuid(), _faker.Commerce.ProductName());

        // Act & Assert
        aggregate.DomainEvents.Should().BeAssignableTo<IReadOnlyCollection<IDomainEvent>>();
    }

    [Fact]
    public void ChangeName_ShouldRaiseDomainEvent()
    {
        // Arrange
        var aggregate = new TestAggregateRoot(Guid.NewGuid(), _faker.Commerce.ProductName());
        var newName = _faker.Commerce.ProductName();

        // Act
        aggregate.ChangeName(newName);

        // Assert
        aggregate.Name.Should().Be(newName);
        aggregate.DomainEvents.Should().HaveCount(1);
        aggregate.DomainEvents.First().Should().BeOfType<TestDomainEvent>();
        
        var domainEvent = aggregate.DomainEvents.First() as TestDomainEvent;
        domainEvent!.EntityId.Should().Be(aggregate.Id);
        domainEvent.Name.Should().Be(newName);
    }

    [Fact]
    public void DomainEvents_AfterMultipleOperations_ShouldAccumulateEvents()
    {
        // Arrange
        var aggregate = new TestAggregateRoot(Guid.NewGuid(), _faker.Commerce.ProductName());

        // Act
        aggregate.ChangeName("Name1");
        aggregate.ChangeName("Name2");
        aggregate.ChangeName("Name3");

        // Assert
        aggregate.DomainEvents.Should().HaveCount(3);
    }

    [Fact]
    public void ParameterlessConstructor_ShouldCreateAggregateForOrmCompatibility()
    {
        // Arrange & Act
        var aggregate = Activator.CreateInstance(typeof(TestAggregateRoot), nonPublic: true) as TestAggregateRoot;

        // Assert
        aggregate.Should().NotBeNull();
        aggregate.DomainEvents.Should().BeEmpty();
    }

    [Fact]
    public void DomainEvents_ShouldNotAllowDirectModification()
    {
        // Arrange
        var aggregate = new TestAggregateRoot(Guid.NewGuid(), _faker.Commerce.ProductName());
        var events = aggregate.DomainEvents;

        // Act & Assert
        events.Should().BeAssignableTo<IReadOnlyCollection<IDomainEvent>>();
        events.GetType().Should().Match(t => 
            t.Name.Contains("ReadOnly") || !t.GetInterfaces().Any(i => i.Name.Contains("IList")));
    }
}
