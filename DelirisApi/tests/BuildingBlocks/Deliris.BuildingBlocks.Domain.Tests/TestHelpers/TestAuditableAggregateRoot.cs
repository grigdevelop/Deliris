namespace Deliris.BuildingBlocks.Domain.Tests.TestHelpers;

public class TestAuditableAggregateRoot : AuditableAggregateRoot<Guid>
{
    public string Name { get; private set; }

    public TestAuditableAggregateRoot(Guid id, string name) : base(id)
    {
        Name = name;
    }

    private TestAuditableAggregateRoot() : base()
    {
        Name = string.Empty;
    }

    public void ChangeName(string name)
    {
        Name = name;
        RaiseDomainEvent(new TestDomainEvent(Id, name));
    }
}
