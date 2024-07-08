using ByReplace.Commands.Apply.Rule;
using ByReplace.Printers;
using ByReplace.Test.ClassFixture;
using Moq;
using Xunit;

namespace ByReplace.Test.Commands.Apply.Rule;

public class ApplyRuleCommandTest : IClassFixture<WorkspaceFixture<ApplyRuleCommandTest>>
{
    private readonly WorkspaceFixture<ApplyRuleCommandTest> _fixture;
    private readonly Mock<IPrint> _printMock;

    public ApplyRuleCommandTest(WorkspaceFixture<ApplyRuleCommandTest> fixture)
    {
        _fixture = fixture;
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

        var command = new ApplyRuleCommand(_fixture.WorkspaceSyntax.BrConfiguration, applyRuleParameter, _printMock.Object);

        // Act
        var executionResult = Record.Exception(() => command.ExecuteAsync());

        // Assert
        Assert.Null(executionResult);
    }
}
