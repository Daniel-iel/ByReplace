using ByReplace.Models;
using ByReplace.Specification.Conditions;
using Moq;
using Xunit;

namespace ByReplace.Test.Specification.Conditions;

public class OrSpecificationTest
{
    [Fact]
    public void IsSatisfiedBy_ShouldReturnTrue_WhenLeftSpecificationIsSatisfied()
    {
        // Arrange
        var mockLeftSpec = new Mock<IMatchSpec>();
        var mockRightSpec = new Mock<IMatchSpec>();

        mockLeftSpec.Setup(spec => spec.IsSatisfiedBy(It.IsAny<Rule>())).Returns(true);
        mockRightSpec.Setup(spec => spec.IsSatisfiedBy(It.IsAny<Rule>())).Returns(false);

        var orSpecification = new OrSpecification(mockLeftSpec.Object, mockRightSpec.Object);

        // Act
        var result = orSpecification.IsSatisfiedBy(It.IsAny<Rule>());

        // Assert
        Assert.True(result);
    }

    [Fact]
    public void IsSatisfiedBy_ShouldReturnTrue_WhenRightSpecificationIsSatisfied()
    {
        // Arrange
        var mockLeftSpec = new Mock<IMatchSpec>();
        var mockRightSpec = new Mock<IMatchSpec>();

        mockLeftSpec.Setup(spec => spec.IsSatisfiedBy(It.IsAny<Rule>())).Returns(false);
        mockRightSpec.Setup(spec => spec.IsSatisfiedBy(It.IsAny<Rule>())).Returns(true);

        var orSpecification = new OrSpecification(mockLeftSpec.Object, mockRightSpec.Object);

        // Act
        var result = orSpecification.IsSatisfiedBy(It.IsAny<Rule>());

        // Assert
        Assert.True(result);
    }

    [Fact]
    public void IsSatisfiedBy_ShouldReturnTrue_WhenBothSpecificationsAreSatisfied()
    {
        // Arrange
        var mockLeftSpec = new Mock<IMatchSpec>();
        var mockRightSpec = new Mock<IMatchSpec>();

        mockLeftSpec.Setup(spec => spec.IsSatisfiedBy(It.IsAny<Rule>())).Returns(true);
        mockRightSpec.Setup(spec => spec.IsSatisfiedBy(It.IsAny<Rule>())).Returns(true);

        var orSpecification = new OrSpecification(mockLeftSpec.Object, mockRightSpec.Object);

        // Act
        var result = orSpecification.IsSatisfiedBy(It.IsAny<Rule>());

        // Assert
        Assert.True(result);
    }

    [Fact]
    public void IsSatisfiedBy_ShouldReturnFalse_WhenNeitherSpecificationIsSatisfied()
    {
        // Arrange
        var mockLeftSpec = new Mock<IMatchSpec>();
        var mockRightSpec = new Mock<IMatchSpec>();

        mockLeftSpec.Setup(spec => spec.IsSatisfiedBy(It.IsAny<Rule>())).Returns(false);
        mockRightSpec.Setup(spec => spec.IsSatisfiedBy(It.IsAny<Rule>())).Returns(false);

        var orSpecification = new OrSpecification(mockLeftSpec.Object, mockRightSpec.Object);

        // Act
        var result = orSpecification.IsSatisfiedBy(It.IsAny<Rule>());

        // Assert
        Assert.False(result);
    }
}
