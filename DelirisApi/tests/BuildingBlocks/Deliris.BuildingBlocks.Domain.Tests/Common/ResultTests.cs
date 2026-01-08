namespace Deliris.BuildingBlocks.Domain.Tests.Common;

[Trait("Category", "Unit")]
[Trait("Component", "Result")]
public class ResultTests
{
    private readonly Faker _faker = new();

    [Fact]
    public void Success_ShouldCreateSuccessfulResult()
    {
        // Act
        var result = Result.Success();

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.IsFailure.Should().BeFalse();
        result.Error.Should().BeEmpty();
    }

    [Fact]
    public void Failure_ShouldCreateFailedResult()
    {
        // Arrange
        var error = _faker.Lorem.Sentence();

        // Act
        var result = Result.Failure(error);

        // Assert
        result.IsSuccess.Should().BeFalse();
        result.IsFailure.Should().BeTrue();
        result.Error.Should().Be(error);
    }

    [Fact]
    public void SuccessWithValue_ShouldCreateSuccessfulResultWithValue()
    {
        // Arrange
        var value = _faker.Random.Int();

        // Act
        var result = Result.Success(value);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.IsFailure.Should().BeFalse();
        result.Value.Should().Be(value);
        result.Error.Should().BeEmpty();
    }

    [Fact]
    public void FailureWithType_ShouldCreateFailedResultWithType()
    {
        // Arrange
        var error = _faker.Lorem.Sentence();

        // Act
        var result = Result.Failure<int>(error);

        // Assert
        result.IsSuccess.Should().BeFalse();
        result.IsFailure.Should().BeTrue();
        result.Error.Should().Be(error);
    }

    [Fact]
    public void Value_OnSuccessfulResult_ShouldReturnValue()
    {
        // Arrange
        var expectedValue = _faker.Random.Int();
        var result = Result.Success(expectedValue);

        // Act
        var value = result.Value;

        // Assert
        value.Should().Be(expectedValue);
    }

    [Fact]
    public void Value_OnFailedResult_ShouldThrowInvalidOperationException()
    {
        // Arrange
        var result = Result.Failure<int>(_faker.Lorem.Sentence());

        // Act
        Action act = () => { var _ = result.Value; };

        // Assert
        act.Should().Throw<InvalidOperationException>()
            .WithMessage("Cannot access the value of a failed result.");
    }

    [Fact]
    public void Constructor_WithSuccessAndError_ShouldThrowInvalidOperationException()
    {
        // Act
        Action act = () => new TestResult(isSuccess: true, error: "Some error");

        // Assert
        act.Should().Throw<InvalidOperationException>()
            .WithMessage("A successful result cannot have an error.");
    }

    [Fact]
    public void Constructor_WithFailureAndNoError_ShouldThrowInvalidOperationException()
    {
        // Act
        Action act = () => new TestResult(isSuccess: false, error: string.Empty);

        // Assert
        act.Should().Throw<InvalidOperationException>()
            .WithMessage("A failed result must have an error.");
    }

    [Fact]
    public void ImplicitConversion_FromValue_ShouldCreateSuccessResult()
    {
        // Arrange
        var value = _faker.Random.Int();

        // Act
        Result<int> result = value;

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Value.Should().Be(value);
    }

    [Theory]
    [InlineData("Error 1")]
    [InlineData("Error 2")]
    [InlineData("Validation failed")]
    public void Failure_WithDifferentErrors_ShouldStoreCorrectError(string error)
    {
        // Act
        var result = Result.Failure(error);

        // Assert
        result.Error.Should().Be(error);
        result.IsFailure.Should().BeTrue();
    }

    [Fact]
    public void Success_WithComplexType_ShouldWorkCorrectly()
    {
        // Arrange
        var complexObject = new { Name = _faker.Name.FullName(), Age = _faker.Random.Int(1, 100) };

        // Act
        var result = Result.Success(complexObject);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Value.Should().Be(complexObject);
    }

    [Fact]
    public void Success_WithNullValue_ShouldAllowNull()
    {
        // Arrange
        string? nullValue = null;

        // Act
        var result = Result.Success(nullValue);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Value.Should().BeNull();
    }

    private class TestResult : Result
    {
        public TestResult(bool isSuccess, string error) : base(isSuccess, error)
        {
        }
    }
}
