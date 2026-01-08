namespace Deliris.BuildingBlocks.Domain.Tests.TestHelpers;

public record TestDomainEvent : DomainEvent
{
    public Guid EntityId { get; }
    public string Name { get; }

    public TestDomainEvent(Guid entityId, string name)
    {
        EntityId = entityId;
        Name = name;
    }
}
