using ByReplace.Commands.Apply.Rules;
using ByReplace.Printers;
using ByReplace.Test.ClassFixture;
using Moq;
using Xunit;

namespace ByReplace.Test.Commands.Apply.Rules;

public class ApplyRulesCommandTest : IClassFixture<WorkspaceFixture>
{
    private readonly WorkspaceFixture _workspace;
    private readonly Mock<IPrint> _printMock;

    public ApplyRulesCommandTest(WorkspaceFixture workspace)
    {
        _workspace = workspace;
        _printMock = new Mock<IPrint>();
    }

    [Fact]
    public void Execute_ApplyRulesToFileThree_DoesNotThrowException()
    {
        // Arrange
        var command = new ApplyRulesCommand(_workspace.BrConfiguration, _printMock.Object);

        // Act
        var executionResult = Record.Exception(() => command.ExecuteAsync(It.IsAny<CancellationToken>()));

        // Assert
        Assert.Null(executionResult);
    }
}
