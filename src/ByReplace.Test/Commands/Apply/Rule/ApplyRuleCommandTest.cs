using ByReplace.Commands.Apply.Rule;
using ByReplace.Printers;
using ByReplace.Test.TestHelpers.ClassFixture;
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

    [Fact]
    public async Task ExecuteAsync_WhenInvoked_AppliesRuleAndModifiesFileAccordingToRule()
    {
        // Arrange
        var applyRuleParameter = new ApplyRuleParameter
        {
            Rule = "RuleTest"
        };

        var command = new ApplyRuleCommand(_fixture.WorkspaceSyntax.BrConfiguration, applyRuleParameter, _printMock.Object);

        // Act
        await command.ExecuteAsync();

        // Assert
        Assert.Collection(_fixture.WorkspaceSyntax.Files,
           entry =>
           {
               string filePath = string.Join(Path.DirectorySeparatorChar, _fixture.WorkspaceSyntax.Identifier, entry.ParentFolder, entry.Name);
               string textModified = File.ReadAllText(filePath);
               Assert.Equal("ITest = new Test()", textModified);
           },
           entry =>
           {
               string filePath = string.Join(Path.DirectorySeparatorChar, _fixture.WorkspaceSyntax.Identifier, entry.ParentFolder, entry.Name);
               string textModified = File.ReadAllText(filePath);
               Assert.Equal("ITest = new Test()", textModified);
           });
    }
}
