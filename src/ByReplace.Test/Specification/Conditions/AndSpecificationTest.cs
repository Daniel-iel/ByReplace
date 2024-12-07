namespace ByReplace.Test.Specification.Conditions;

public class AndSpecificationTest
{
    [Fact]
    public void IsSatisfiedBy_ShouldReturnTrue_WhenBothSpecificationsAreSatisfied()
    {
        // Arrange
        var mockLeftSpec = new Mock<IMatchSpecification>();
        var mockRightSpec = new Mock<IMatchSpecification>();

        mockLeftSpec.Setup(spec => spec.IsSatisfiedBy(It.IsAny<FileMapper>(), It.IsAny<Rule>())).Returns(true);
        mockRightSpec.Setup(spec => spec.IsSatisfiedBy(It.IsAny<FileMapper>(), It.IsAny<Rule>())).Returns(true);

        var andSpecification = new AndSpecification(mockLeftSpec.Object, mockRightSpec.Object);

        // Act
        var result = andSpecification.IsSatisfiedBy(It.IsAny<FileMapper>(), It.IsAny<Rule>());

        // Assert
        Assert.True(result);
    }

    [Fact]
    public void IsSatisfiedBy_ShouldReturnFalse_WhenLeftSpecificationIsNotSatisfied()
    {
        // Arrange
        var mockLeftSpec = new Mock<IMatchSpecification>();
        var mockRightSpec = new Mock<IMatchSpecification>();

        mockLeftSpec.Setup(spec => spec.IsSatisfiedBy(It.IsAny<FileMapper>(), It.IsAny<Rule>())).Returns(false);
        mockRightSpec.Setup(spec => spec.IsSatisfiedBy(It.IsAny<FileMapper>(), It.IsAny<Rule>())).Returns(true);

        var andSpecification = new AndSpecification(mockLeftSpec.Object, mockRightSpec.Object);

        // Act
        var result = andSpecification.IsSatisfiedBy(It.IsAny<FileMapper>(), It.IsAny<Rule>());

        // Assert
        Assert.False(result);
    }

    [Fact]
    public void IsSatisfiedBy_ShouldReturnFalse_WhenRightSpecificationIsNotSatisfied()
    {
        // Arrange
        var mockLeftSpec = new Mock<IMatchSpecification>();
        var mockRightSpec = new Mock<IMatchSpecification>();

        mockLeftSpec.Setup(spec => spec.IsSatisfiedBy(It.IsAny<FileMapper>(), It.IsAny<Rule>())).Returns(true);
        mockRightSpec.Setup(spec => spec.IsSatisfiedBy(It.IsAny<FileMapper>(), It.IsAny<Rule>())).Returns(false);

        var andSpecification = new AndSpecification(mockLeftSpec.Object, mockRightSpec.Object);

        // Act
        var result = andSpecification.IsSatisfiedBy(It.IsAny<FileMapper>(), It.IsAny<Rule>());

        // Assert
        Assert.False(result);
    }

    [Fact]
    public void IsSatisfiedBy_ShouldReturnFalse_WhenBothSpecificationsAreNotSatisfied()
    {
        // Arrange
        var mockLeftSpec = new Mock<IMatchSpecification>();
        var mockRightSpec = new Mock<IMatchSpecification>();

        mockLeftSpec.Setup(spec => spec.IsSatisfiedBy(It.IsAny<FileMapper>(), It.IsAny<Rule>())).Returns(false);
        mockRightSpec.Setup(spec => spec.IsSatisfiedBy(It.IsAny<FileMapper>(), It.IsAny<Rule>())).Returns(false);

        var andSpecification = new AndSpecification(mockLeftSpec.Object, mockRightSpec.Object);

        // Act
        var result = andSpecification.IsSatisfiedBy(It.IsAny<FileMapper>(), It.IsAny<Rule>());

        // Assert
        Assert.False(result);
    }
}
