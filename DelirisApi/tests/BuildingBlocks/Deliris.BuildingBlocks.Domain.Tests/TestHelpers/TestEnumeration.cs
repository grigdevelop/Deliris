namespace Deliris.BuildingBlocks.Domain.Tests.TestHelpers;

public class TestEnumeration : Enumeration
{
    public static readonly TestEnumeration First = new(1, "First");
    public static readonly TestEnumeration Second = new(2, "Second");
    public static readonly TestEnumeration Third = new(3, "Third");

    private TestEnumeration(int id, string name) : base(id, name)
    {
    }
}
