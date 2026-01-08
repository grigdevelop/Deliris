namespace Deliris.BuildingBlocks.Domain.Tests.TestHelpers;

public class TestSoftDeletableEntity : Entity<Guid>, ISoftDeletable
{
    public string Name { get; private set; }
    public bool IsDeleted { get; private set; }
    public DateTime? DeletedAtUtc { get; private set; }
    public string? DeletedBy { get; private set; }

    public TestSoftDeletableEntity(Guid id, string name) : base(id)
    {
        Name = name;
    }

    private TestSoftDeletableEntity() : base()
    {
        Name = string.Empty;
    }

    public void Delete(string? deletedBy, DateTime? deletedAtUtc = null)
    {
        IsDeleted = true;
        DeletedBy = deletedBy;
        DeletedAtUtc = deletedAtUtc ?? DateTime.UtcNow;
    }

    public void Restore()
    {
        IsDeleted = false;
        DeletedBy = null;
        DeletedAtUtc = null;
    }
}
