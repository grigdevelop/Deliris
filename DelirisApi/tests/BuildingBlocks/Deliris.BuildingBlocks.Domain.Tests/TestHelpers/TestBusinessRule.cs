namespace Deliris.BuildingBlocks.Domain.Tests.TestHelpers;

public class TestBusinessRule : IBusinessRule
{
    private readonly bool _isSatisfied;

    public string Name { get; }
    public string Message { get; }

    public TestBusinessRule(bool isSatisfied, string name = "TestRule", string message = "Test rule violated")
    {
        _isSatisfied = isSatisfied;
        Name = name;
        Message = message;
    }

    public bool IsSatisfied() => _isSatisfied;
}
