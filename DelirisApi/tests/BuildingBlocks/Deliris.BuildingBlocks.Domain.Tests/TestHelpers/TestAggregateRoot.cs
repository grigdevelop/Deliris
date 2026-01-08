namespace Deliris.BuildingBlocks.Domain.Tests.TestHelpers;

public class TestAggregateRoot : AggregateRoot<Guid>
{
    public string Name { get; private set; }

    public TestAggregateRoot(Guid id, string name) : base(id)
    {
        Name = name;
    }

    private TestAggregateRoot() : base()
    {
        Name = string.Empty;
    }

    public void ChangeName(string name)
    {
        Name = name;
        RaiseDomainEvent(new TestDomainEvent(Id, name));
    }

    public void RaiseTestEvent(IDomainEvent domainEvent)
    {
        RaiseDomainEvent(domainEvent);
    }
}
