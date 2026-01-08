# Deliris.BuildingBlocks.Domain.Tests

Comprehensive unit test suite for the Deliris.BuildingBlocks.Domain library using xUnit, FluentAssertions, Moq, and Bogus.

## Project Information

- **Framework**: .NET 10.0
- **Test Framework**: xUnit 2.9.3
- **Assertion Library**: FluentAssertions 8.8.0
- **Mocking Framework**: Moq 4.20.72
- **Test Data Generator**: Bogus 35.6.1
- **Code Coverage**: coverlet.collector 6.0.2

## Test Structure

### Test Organization

Tests are organized by component and follow the pattern: `MethodName_Scenario_ExpectedBehavior`

```
Deliris.BuildingBlocks.Domain.Tests/
├── Common/
│   ├── PagedListTests.cs
│   └── ResultTests.cs
├── Exceptions/
│   └── DomainExceptionTests.cs
├── Primitives/
│   ├── AggregateRootTests.cs
│   ├── AuditableAggregateRootTests.cs
│   ├── AuditableEntityTests.cs
│   ├── BusinessRuleTests.cs
│   ├── DomainEventTests.cs
│   ├── EntityTests.cs
│   ├── EnumerationTests.cs
│   ├── SoftDeletableTests.cs
│   └── ValueObjectTests.cs
├── Specifications/
│   └── SpecificationTests.cs
└── TestHelpers/
    ├── TestAggregateRoot.cs
    ├── TestAuditableAggregateRoot.cs
    ├── TestAuditableEntity.cs
    ├── TestBusinessRule.cs
    ├── TestDomainEvent.cs
    ├── TestEntity.cs
    ├── TestEnumeration.cs
    ├── TestSoftDeletableEntity.cs
    ├── TestSpecification.cs
    └── TestValueObject.cs
```

## Test Coverage

### 1. Entity Tests (EntityTests.cs)
- ✅ Entity creation and initialization
- ✅ Identity-based equality comparison
- ✅ GetHashCode consistency
- ✅ Equality operators (==, !=)
- ✅ Null handling
- ✅ Type checking in equality
- ✅ ORM compatibility (parameterless constructor)
- ✅ Same ID with different properties equality

**Total Tests**: 16

### 2. ValueObject Tests (ValueObjectTests.cs)
- ✅ Structural equality comparison
- ✅ Immutability verification
- ✅ GetHashCode with same/different values
- ✅ Equality operators
- ✅ Null handling
- ✅ Type checking
- ✅ Reference equality

**Total Tests**: 14

### 3. AggregateRoot Tests (AggregateRootTests.cs)
- ✅ Domain event raising
- ✅ Domain event clearing
- ✅ Event collection immutability
- ✅ Multiple event handling
- ✅ Event ordering
- ✅ Null event handling
- ✅ Read-only event collection
- ✅ ORM compatibility

**Total Tests**: 10

### 4. AuditableEntity Tests (AuditableEntityTests.cs)
- ✅ CreatedAtUtc initialization
- ✅ SetCreatedAudit method
- ✅ SetUpdatedAudit method
- ✅ UTC datetime handling
- ✅ Null user handling
- ✅ IAuditableEntity interface implementation
- ✅ Multiple updates
- ✅ ORM compatibility

**Total Tests**: 11

### 5. AuditableAggregateRoot Tests (AuditableAggregateRootTests.cs)
- ✅ Combined audit and domain event functionality
- ✅ Interface implementations
- ✅ Inheritance verification
- ✅ Domain events with audit fields

**Total Tests**: 7

### 6. DomainEvent Tests (DomainEventTests.cs)
- ✅ Event creation with timestamp
- ✅ Unique ID generation
- ✅ Immutability (record type)
- ✅ IDomainEvent interface implementation
- ✅ UTC timestamp verification
- ✅ Value equality for records
- ✅ Timestamp ordering

**Total Tests**: 9

### 7. Business Rule Tests (BusinessRuleTests.cs)
- ✅ CheckRule with satisfied rules
- ✅ CheckRule with unsatisfied rules (exception throwing)
- ✅ CheckRules with multiple rules
- ✅ Validate method
- ✅ Null handling
- ✅ Custom business rule implementations

**Total Tests**: 10

### 8. Specification Tests (SpecificationTests.cs)
- ✅ Criteria building
- ✅ Include expressions (navigation properties)
- ✅ OrderBy and OrderByDescending
- ✅ GroupBy expressions
- ✅ Pagination (Skip/Take)
- ✅ Tracking configuration
- ✅ Split query configuration
- ✅ Specification composition (And, Or, Not)

**Total Tests**: 18

### 9. Result Pattern Tests (ResultTests.cs)
- ✅ Success result creation
- ✅ Failure result creation
- ✅ Result<T> with value
- ✅ Value access on success/failure
- ✅ IsSuccess and IsFailure properties
- ✅ Error access
- ✅ Implicit conversions
- ✅ Constructor validation
- ✅ Complex type support
- ✅ Null value handling

**Total Tests**: 11

### 10. Enumeration Tests (EnumerationTests.cs)
- ✅ GetAll method
- ✅ FromId retrieval
- ✅ FromName retrieval
- ✅ TryFromId method
- ✅ TryFromName method
- ✅ Equality comparison
- ✅ GetHashCode consistency
- ✅ ToString method
- ✅ CompareTo method
- ✅ Invalid id/name handling

**Total Tests**: 18

### 11. Domain Exception Tests (DomainExceptionTests.cs)
- ✅ BusinessRuleValidationException
- ✅ EntityNotFoundException
- ✅ InvalidEntityStateException
- ✅ DomainValidationException
- ✅ Error code verification
- ✅ Inner exception handling
- ✅ Single and multiple validation errors
- ✅ Exception hierarchy

**Total Tests**: 15

### 12. ISoftDeletable Tests (SoftDeletableTests.cs)
- ✅ Delete method (IsDeleted, DeletedAtUtc, DeletedBy)
- ✅ Restore method
- ✅ Soft delete state transitions
- ✅ Null user handling
- ✅ Custom timestamp support
- ✅ Multiple delete operations

**Total Tests**: 13

### 13. PagedList Tests (PagedListTests.cs)
- ✅ Pagination metadata (PageNumber, PageSize, TotalCount, TotalPages)
- ✅ HasPreviousPage and HasNextPage
- ✅ Empty page creation
- ✅ Create method with pagination
- ✅ Parameter validation
- ✅ Read-only items collection

**Total Tests**: 14

## Total Test Count: **166 Tests**

## Running Tests

### Command Line
```bash
# Run all tests
dotnet test

# Run with code coverage
dotnet test /p:CollectCoverage=true /p:CoverletOutputFormat=opencover

# Run specific test class
dotnet test --filter "FullyQualifiedName~EntityTests"

# Run tests by trait
dotnet test --filter "Category=Unit"
dotnet test --filter "Component=Entity"
```

### Visual Studio
- Open Test Explorer (Test > Test Explorer)
- Click "Run All" or select specific tests to run

## Test Patterns Used

### Arrange-Act-Assert (AAA)
All tests follow the AAA pattern for clarity:
```csharp
[Fact]
public void Method_Scenario_ExpectedBehavior()
{
    // Arrange - Set up test data and dependencies
    var entity = new TestEntity(Guid.NewGuid(), "Test");
    
    // Act - Execute the method under test
    var result = entity.DoSomething();
    
    // Assert - Verify the expected outcome
    result.Should().BeTrue();
}
```

### Theory Tests
Parameterized tests for testing multiple scenarios:
```csharp
[Theory]
[InlineData("value1")]
[InlineData("value2")]
public void Method_WithDifferentInputs_ShouldWork(string input)
{
    // Test implementation
}
```

### Traits
Tests are categorized using traits:
```csharp
[Trait("Category", "Unit")]
[Trait("Component", "Entity")]
```

## Test Data Generation

Using Bogus for realistic test data:
```csharp
private readonly Faker _faker = new();

var name = _faker.Commerce.ProductName();
var email = _faker.Internet.Email();
var address = _faker.Address.StreetAddress();
```

## Code Coverage Goals

- **Target**: Minimum 90% code coverage
- **Focus**: Business logic and edge cases
- **Exclusions**: Trivial getters/setters

## Best Practices

1. **Descriptive Test Names**: Follow `MethodName_Scenario_ExpectedBehavior` pattern
2. **Single Assertion Focus**: Each test verifies one specific behavior
3. **Test Independence**: Tests don't depend on each other
4. **Fast Execution**: All tests are unit tests with no external dependencies
5. **Readable Assertions**: Use FluentAssertions for clear, readable assertions
6. **Comprehensive Coverage**: Test happy paths, edge cases, and error conditions

## Continuous Integration

These tests are designed to run in CI/CD pipelines:
- Fast execution (no database or external dependencies)
- Deterministic results
- Clear failure messages
- Code coverage reporting support

## Contributing

When adding new domain components:
1. Create corresponding test helper classes in `TestHelpers/`
2. Add comprehensive test class following existing patterns
3. Ensure minimum 90% code coverage
4. Use descriptive test names
5. Add appropriate traits for categorization
6. Update this README with new test counts

## License

This test project is part of the Deliris solution.
