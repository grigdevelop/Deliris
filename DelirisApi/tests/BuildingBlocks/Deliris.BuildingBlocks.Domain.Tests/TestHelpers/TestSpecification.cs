using Deliris.BuildingBlocks.Domain.Tests.TestHelpers;

namespace Deliris.BuildingBlocks.Domain.Tests.TestHelpers;

public class TestSpecification : Specification<TestEntity>
{
    public TestSpecification(string nameFilter)
    {
        AddCriteria(e => e.Name.Contains(nameFilter));
    }

    public TestSpecification()
    {
    }

    public void AddTestCriteria(System.Linq.Expressions.Expression<Func<TestEntity, bool>> criteria)
    {
        AddCriteria(criteria);
    }

    public void AddTestInclude(System.Linq.Expressions.Expression<Func<TestEntity, object>> include)
    {
        AddInclude(include);
    }

    public void AddTestInclude(string includeString)
    {
        AddInclude(includeString);
    }

    public void AddTestOrderBy(System.Linq.Expressions.Expression<Func<TestEntity, object>> orderBy)
    {
        AddOrderBy(orderBy);
    }

    public void AddTestOrderByDescending(System.Linq.Expressions.Expression<Func<TestEntity, object>> orderByDesc)
    {
        AddOrderByDescending(orderByDesc);
    }

    public void AddTestGroupBy(System.Linq.Expressions.Expression<Func<TestEntity, object>> groupBy)
    {
        AddGroupBy(groupBy);
    }

    public void ApplyTestPaging(int skip, int take)
    {
        ApplyPaging(skip, take);
    }

    public void SetNoTracking()
    {
        AsNoTracking();
    }

    public void SetSplitQuery()
    {
        AsSplitQuery();
    }
}
