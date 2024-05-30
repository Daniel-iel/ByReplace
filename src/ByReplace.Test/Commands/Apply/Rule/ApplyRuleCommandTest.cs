using ByReplace.Commands.Apply.Rule;
using ByReplace.Printers;
using ByReplace.Test.ClassFixture;
using Moq;
using Xunit;

namespace ByReplace.Test.Commands.Apply.Rule;

public class ApplyRuleCommandTest : IClassFixture<WorkspaceFixture>
{
    private readonly WorkspaceFixture _workspace;
    private readonly Mock<IPrint> _printMock;

    public ApplyRuleCommandTest(WorkspaceFixture workspace)
    {
        _workspace = workspace;
        _printMock = new Mock<IPrint>();
    }

    [Fact]
    public void Execute_ApplyIndividualRuleToFileThree_DoesNotThrowException()
    {
        // Arrange
        var applyRuleParameter = new ApplyRuleParameter
        {
            Rule = "RuleTest"
        };

        var command = new ApplyRuleCommand(_workspace.BrConfiguration, applyRuleParameter, _printMock.Object);

        // Act
        var executionResult = Record.Exception(() => command.ExecuteAsync());

        // Assert
        Assert.Null(executionResult);
    }
}
