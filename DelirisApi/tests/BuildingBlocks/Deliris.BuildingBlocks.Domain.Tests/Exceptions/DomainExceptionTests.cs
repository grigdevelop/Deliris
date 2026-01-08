using Deliris.BuildingBlocks.Domain.Abstractions.Exceptions;

namespace Deliris.BuildingBlocks.Domain.Tests.Exceptions;

[Trait("Category", "Unit")]
[Trait("Component", "DomainException")]
public class DomainExceptionTests
{
    private readonly Faker _faker = new();

    [Fact]
    public void BusinessRuleValidationException_ShouldSetRuleNameAndMessage()
    {
        // Arrange
        var ruleName = "TestRule";
        var message = _faker.Lorem.Sentence();

        // Act
        var exception = new BusinessRuleValidationException(ruleName, message);

        // Assert
        exception.RuleName.Should().Be(ruleName);
        exception.Message.Should().Be(message);
        exception.ErrorCode.Should().Be("BUSINESS_RULE_VIOLATION");
    }

    [Fact]
    public void BusinessRuleValidationException_WithInnerException_ShouldSetInnerException()
    {
        // Arrange
        var ruleName = "TestRule";
        var message = _faker.Lorem.Sentence();
        var innerException = new Exception("Inner exception");

        // Act
        var exception = new BusinessRuleValidationException(ruleName, message, innerException);

        // Assert
        exception.RuleName.Should().Be(ruleName);
        exception.Message.Should().Be(message);
        exception.InnerException.Should().Be(innerException);
    }

    [Fact]
    public void EntityNotFoundException_ShouldSetEntityTypeAndId()
    {
        // Arrange
        var entityType = typeof(string);
        var entityId = Guid.NewGuid();

        // Act
        var exception = new EntityNotFoundException(entityType, entityId);

        // Assert
        exception.EntityType.Should().Be(entityType);
        exception.EntityId.Should().Be(entityId);
        exception.ErrorCode.Should().Be("ENTITY_NOT_FOUND");
        exception.Message.Should().Contain(entityType.Name);
        exception.Message.Should().Contain(entityId.ToString());
    }

    [Fact]
    public void EntityNotFoundException_WithInnerException_ShouldSetInnerException()
    {
        // Arrange
        var entityType = typeof(string);
        var entityId = Guid.NewGuid();
        var innerException = new Exception("Inner exception");

        // Act
        var exception = new EntityNotFoundException(entityType, entityId, innerException);

        // Assert
        exception.EntityType.Should().Be(entityType);
        exception.EntityId.Should().Be(entityId);
        exception.InnerException.Should().Be(innerException);
    }

    [Fact]
    public void InvalidEntityStateException_ShouldSetEntityTypeIdAndMessage()
    {
        // Arrange
        var entityType = typeof(string);
        var entityId = Guid.NewGuid();
        var message = _faker.Lorem.Sentence();

        // Act
        var exception = new InvalidEntityStateException(entityType, entityId, message);

        // Assert
        exception.EntityType.Should().Be(entityType);
        exception.EntityId.Should().Be(entityId);
        exception.Message.Should().Be(message);
        exception.ErrorCode.Should().Be("INVALID_ENTITY_STATE");
    }

    [Fact]
    public void InvalidEntityStateException_WithNullEntityId_ShouldAllowNull()
    {
        // Arrange
        var entityType = typeof(string);
        object? entityId = null;
        var message = _faker.Lorem.Sentence();

        // Act
        var exception = new InvalidEntityStateException(entityType, entityId, message);

        // Assert
        exception.EntityType.Should().Be(entityType);
        exception.EntityId.Should().BeNull();
        exception.Message.Should().Be(message);
    }

    [Fact]
    public void InvalidEntityStateException_WithInnerException_ShouldSetInnerException()
    {
        // Arrange
        var entityType = typeof(string);
        var entityId = Guid.NewGuid();
        var message = _faker.Lorem.Sentence();
        var innerException = new Exception("Inner exception");

        // Act
        var exception = new InvalidEntityStateException(entityType, entityId, message, innerException);

        // Assert
        exception.InnerException.Should().Be(innerException);
    }

    [Fact]
    public void DomainValidationException_WithSingleError_ShouldSetError()
    {
        // Arrange
        var propertyName = "PropertyName";
        var errorMessage = _faker.Lorem.Sentence();

        // Act
        var exception = new DomainValidationException(propertyName, errorMessage);

        // Assert
        exception.Errors.Should().ContainKey(propertyName);
        exception.Errors[propertyName].Should().Contain(errorMessage);
        exception.ErrorCode.Should().Be("VALIDATION_ERROR");
        exception.Message.Should().Contain(propertyName);
        exception.Message.Should().Contain(errorMessage);
    }

    [Fact]
    public void DomainValidationException_WithMultipleErrors_ShouldSetAllErrors()
    {
        // Arrange
        var errors = new Dictionary<string, string[]>
        {
            { "Property1", new[] { "Error1", "Error2" } },
            { "Property2", new[] { "Error3" } }
        };

        // Act
        var exception = new DomainValidationException(errors);

        // Assert
        exception.Errors.Should().HaveCount(2);
        exception.Errors["Property1"].Should().HaveCount(2);
        exception.Errors["Property2"].Should().HaveCount(1);
        exception.ErrorCode.Should().Be("VALIDATION_ERROR");
    }

    [Fact]
    public void DomainException_ShouldBeBaseClass()
    {
        // Arrange & Act
        var businessRuleException = new BusinessRuleValidationException("Rule", "Message");
        var entityNotFoundException = new EntityNotFoundException(typeof(string), Guid.NewGuid());
        var invalidStateException = new InvalidEntityStateException(typeof(string), Guid.NewGuid(), "Message");
        var validationException = new DomainValidationException("Property", "Error");

        // Assert
        businessRuleException.Should().BeAssignableTo<DomainException>();
        entityNotFoundException.Should().BeAssignableTo<DomainException>();
        invalidStateException.Should().BeAssignableTo<DomainException>();
        validationException.Should().BeAssignableTo<DomainException>();
    }

    [Theory]
    [InlineData("BUSINESS_RULE_VIOLATION")]
    [InlineData("ENTITY_NOT_FOUND")]
    [InlineData("INVALID_ENTITY_STATE")]
    [InlineData("VALIDATION_ERROR")]
    public void DomainException_ShouldHaveErrorCode(string expectedErrorCode)
    {
        // Arrange
        DomainException exception = expectedErrorCode switch
        {
            "BUSINESS_RULE_VIOLATION" => new BusinessRuleValidationException("Rule", "Message"),
            "ENTITY_NOT_FOUND" => new EntityNotFoundException(typeof(string), Guid.NewGuid()),
            "INVALID_ENTITY_STATE" => new InvalidEntityStateException(typeof(string), Guid.NewGuid(), "Message"),
            "VALIDATION_ERROR" => new DomainValidationException("Property", "Error"),
            _ => throw new ArgumentException("Invalid error code")
        };

        // Act & Assert
        exception.ErrorCode.Should().Be(expectedErrorCode);
    }

    [Fact]
    public void DomainValidationException_Errors_ShouldBeReadOnly()
    {
        // Arrange
        var errors = new Dictionary<string, string[]>
        {
            { "Property1", new[] { "Error1" } }
        };
        var exception = new DomainValidationException(errors);

        // Act & Assert
        exception.Errors.Should().BeAssignableTo<IReadOnlyDictionary<string, string[]>>();
    }
}
