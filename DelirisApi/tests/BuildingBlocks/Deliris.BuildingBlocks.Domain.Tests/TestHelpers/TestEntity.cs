namespace Deliris.BuildingBlocks.Domain.Tests.TestHelpers;

public class TestEntity : Entity<Guid>
{
    public string Name { get; private set; }

    public TestEntity(Guid id, string name) : base(id)
    {
        Name = name;
    }

    private TestEntity() : base()
    {
        Name = string.Empty;
    }

    public void ChangeName(string name)
    {
        Name = name;
    }
}
