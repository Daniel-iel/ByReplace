using ByReplace.Models;
using ByReplace.Specification.Conditions;
using Moq;
using Xunit;

namespace ByReplace.Test.Specification.Conditions;

public class AndSpecificationTest
{
    [Fact]
    public void IsSatisfiedBy_ShouldReturnTrue_WhenBothSpecificationsAreSatisfied()
    {
        // Arrange
        var mockLeftSpec = new Mock<IMatchSpec>();
        var mockRightSpec = new Mock<IMatchSpec>();

        mockLeftSpec.Setup(spec => spec.IsSatisfiedBy(It.IsAny<Rule>())).Returns(true);
        mockRightSpec.Setup(spec => spec.IsSatisfiedBy(It.IsAny<Rule>())).Returns(true);

        var andSpecification = new AndSpecification(mockLeftSpec.Object, mockRightSpec.Object);

        // Act
        var result = andSpecification.IsSatisfiedBy(It.IsAny<Rule>());

        // Assert
        Assert.True(result);
    }

    [Fact]
    public void IsSatisfiedBy_ShouldReturnFalse_WhenLeftSpecificationIsNotSatisfied()
    {
        // Arrange
        var mockLeftSpec = new Mock<IMatchSpec>();
        var mockRightSpec = new Mock<IMatchSpec>();

        mockLeftSpec.Setup(spec => spec.IsSatisfiedBy(It.IsAny<Rule>())).Returns(false);
        mockRightSpec.Setup(spec => spec.IsSatisfiedBy(It.IsAny<Rule>())).Returns(true);

        var andSpecification = new AndSpecification(mockLeftSpec.Object, mockRightSpec.Object);

        // Act
        var result = andSpecification.IsSatisfiedBy(It.IsAny<Rule>());

        // Assert
        Assert.False(result);
    }

    [Fact]
    public void IsSatisfiedBy_ShouldReturnFalse_WhenRightSpecificationIsNotSatisfied()
    {
        // Arrange
        var mockLeftSpec = new Mock<IMatchSpec>();
        var mockRightSpec = new Mock<IMatchSpec>();

        mockLeftSpec.Setup(spec => spec.IsSatisfiedBy(It.IsAny<Rule>())).Returns(true);
        mockRightSpec.Setup(spec => spec.IsSatisfiedBy(It.IsAny<Rule>())).Returns(false);

        var andSpecification = new AndSpecification(mockLeftSpec.Object, mockRightSpec.Object);

        // Act
        var result = andSpecification.IsSatisfiedBy(It.IsAny<Rule>());

        // Assert
        Assert.False(result);
    }

    [Fact]
    public void IsSatisfiedBy_ShouldReturnFalse_WhenBothSpecificationsAreNotSatisfied()
    {
        // Arrange
        var mockLeftSpec = new Mock<IMatchSpec>();
        var mockRightSpec = new Mock<IMatchSpec>();

        mockLeftSpec.Setup(spec => spec.IsSatisfiedBy(It.IsAny<Rule>())).Returns(false);
        mockRightSpec.Setup(spec => spec.IsSatisfiedBy(It.IsAny<Rule>())).Returns(false);

        var andSpecification = new AndSpecification(mockLeftSpec.Object, mockRightSpec.Object);

        // Act
        var result = andSpecification.IsSatisfiedBy(It.IsAny<Rule>());

        // Assert
        Assert.False(result);
    }
}
