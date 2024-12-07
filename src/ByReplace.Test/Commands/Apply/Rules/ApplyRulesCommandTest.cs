using ByReplace.Test.TestHelpers.ClassFixture;

namespace ByReplace.Test.Commands.Apply.Rules;

public class ApplyRulesCommandTest : IClassFixture<WorkspaceFixture<ApplyRulesCommandTest>>
{
    private readonly WorkspaceFixture<ApplyRulesCommandTest> _fixture;
    private readonly Mock<IPrint> _printMock;

    public ApplyRulesCommandTest(WorkspaceFixture<ApplyRulesCommandTest> fixture)
    {
        _fixture = fixture;
        _printMock = new Mock<IPrint>();
    }

    [Fact]
    public void Execute_ApplyRulesToFileThree_DoesNotThrowException()
    {
        // Arrange
        var command = new ApplyRulesCommand(_fixture.WorkspaceSyntax.BrConfiguration, _printMock.Object);

        // Act
        var executionResult = Record.Exception(() => command.ExecuteAsync(It.IsAny<CancellationToken>()));

        // Assert
        Assert.Null(executionResult);
    }

    [Fact]
    public async Task ExecuteAsync_WhenInvoked_AppliesRulesAndModifiesFileAccordingToRule()
    {
        // Arrange
        var command = new ApplyRulesCommand(_fixture.WorkspaceSyntax.BrConfiguration, _printMock.Object);

        // Act
        await command.ExecuteAsync(It.IsAny<CancellationToken>());

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
