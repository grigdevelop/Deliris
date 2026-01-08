namespace Deliris.BuildingBlocks.Domain.Tests.TestHelpers;

public class TestAuditableEntity : AuditableEntity<Guid>
{
    public string Name { get; private set; }

    public TestAuditableEntity(Guid id, string name) : base(id)
    {
        Name = name;
    }

    private TestAuditableEntity() : base()
    {
        Name = string.Empty;
    }

    public void ChangeName(string name)
    {
        Name = name;
    }
}
