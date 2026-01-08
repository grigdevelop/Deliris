using Deliris.BuildingBlocks.Domain.Tests.TestHelpers;

namespace Deliris.BuildingBlocks.Domain.Tests.Primitives;

[Trait("Category", "Unit")]
[Trait("Component", "BusinessRule")]
public class BusinessRuleTests
{
    [Fact]
    public void CheckRule_WithSatisfiedRule_ShouldNotThrowException()
    {
        // Arrange
        var rule = new TestBusinessRule(isSatisfied: true);

        // Act
        Action act = () => BusinessRuleValidator.CheckRule(rule);

        // Assert
        act.Should().NotThrow();
    }

    [Fact]
    public void CheckRule_WithUnsatisfiedRule_ShouldThrowBusinessRuleValidationException()
    {
        // Arrange
        var ruleName = "TestRule";
        var message = "Test rule violated";
        var rule = new TestBusinessRule(isSatisfied: false, name: ruleName, message: message);

        // Act
        Action act = () => BusinessRuleValidator.CheckRule(rule);

        // Assert
        act.Should().Throw<BusinessRuleValidationException>()
            .WithMessage(message)
            .And.RuleName.Should().Be(ruleName);
    }

    [Fact]
    public void CheckRule_WithNullRule_ShouldThrowArgumentNullException()
    {
        // Arrange
        IBusinessRule rule = null!;

        // Act
        Action act = () => BusinessRuleValidator.CheckRule(rule);

        // Assert
        act.Should().Throw<ArgumentNullException>();
    }

    [Fact]
    public void CheckRules_WithAllSatisfiedRules_ShouldNotThrowException()
    {
        // Arrange
        var rule1 = new TestBusinessRule(isSatisfied: true, name: "Rule1");
        var rule2 = new TestBusinessRule(isSatisfied: true, name: "Rule2");
        var rule3 = new TestBusinessRule(isSatisfied: true, name: "Rule3");

        // Act
        Action act = () => BusinessRuleValidator.CheckRules(rule1, rule2, rule3);

        // Assert
        act.Should().NotThrow();
    }

    [Fact]
    public void CheckRules_WithOneUnsatisfiedRule_ShouldThrowBusinessRuleValidationException()
    {
        // Arrange
        var rule1 = new TestBusinessRule(isSatisfied: true, name: "Rule1");
        var rule2 = new TestBusinessRule(isSatisfied: false, name: "Rule2", message: "Rule2 violated");
        var rule3 = new TestBusinessRule(isSatisfied: true, name: "Rule3");

        // Act
        Action act = () => BusinessRuleValidator.CheckRules(rule1, rule2, rule3);

        // Assert
        act.Should().Throw<BusinessRuleValidationException>()
            .WithMessage("Rule2 violated")
            .And.RuleName.Should().Be("Rule2");
    }

    [Fact]
    public void CheckRules_WithNullRulesArray_ShouldThrowArgumentNullException()
    {
        // Arrange
        IBusinessRule[] rules = null!;

        // Act
        Action act = () => BusinessRuleValidator.CheckRules(rules);

        // Assert
        act.Should().Throw<ArgumentNullException>();
    }

    [Fact]
    public void Validate_WithSatisfiedRule_ShouldReturnTrue()
    {
        // Arrange
        var rule = new TestBusinessRule(isSatisfied: true);

        // Act
        var result = BusinessRuleValidator.Validate(rule);

        // Assert
        result.Should().BeTrue();
    }

    [Fact]
    public void Validate_WithUnsatisfiedRule_ShouldReturnFalse()
    {
        // Arrange
        var rule = new TestBusinessRule(isSatisfied: false);

        // Act
        var result = BusinessRuleValidator.Validate(rule);

        // Assert
        result.Should().BeFalse();
    }

    [Fact]
    public void Validate_WithNullRule_ShouldThrowArgumentNullException()
    {
        // Arrange
        IBusinessRule rule = null!;

        // Act
        Action act = () => BusinessRuleValidator.Validate(rule);

        // Assert
        act.Should().Throw<ArgumentNullException>();
    }

    [Theory]
    [InlineData(true)]
    [InlineData(false)]
    public void IsSatisfied_ShouldReturnCorrectValue(bool expectedResult)
    {
        // Arrange
        var rule = new TestBusinessRule(isSatisfied: expectedResult);

        // Act
        var result = rule.IsSatisfied();

        // Assert
        result.Should().Be(expectedResult);
    }

    [Fact]
    public void BusinessRule_ShouldHaveNameAndMessage()
    {
        // Arrange
        var name = "CustomRule";
        var message = "Custom rule message";

        // Act
        var rule = new TestBusinessRule(isSatisfied: true, name: name, message: message);

        // Assert
        rule.Name.Should().Be(name);
        rule.Message.Should().Be(message);
    }
}
