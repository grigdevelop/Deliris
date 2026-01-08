namespace Deliris.BuildingBlocks.Domain.Tests.TestHelpers;

public class TestValueObject : ValueObject
{
    public string Street { get; }
    public string City { get; }
    public string ZipCode { get; }

    public TestValueObject(string street, string city, string zipCode)
    {
        Street = street;
        City = city;
        ZipCode = zipCode;
    }

    protected override IEnumerable<object?> GetEqualityComponents()
    {
        yield return Street;
        yield return City;
        yield return ZipCode;
    }
}
