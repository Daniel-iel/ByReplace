using ByReplace.Commands.Rule.ListRules;
using ByReplace.Printers;
using ByReplace.Test.ClassFixture;
using Moq;
using Xunit;

namespace ByReplace.Test.Commands.Rule.ListRules;

public class ListRulesCommandTest : IClassFixture<WorkspaceFixture<ListRulesCommandTest>>
{
    private readonly WorkspaceFixture<ListRulesCommandTest> _fixture;
    private readonly Mock<IPrintBox> _printBoxMock;

    public ListRulesCommandTest(WorkspaceFixture<ListRulesCommandTest> fixture)
    {
        _fixture = fixture;
        _printBoxMock = new Mock<IPrintBox>();
    }

    [Fact]
    public async Task Execute_WhenPrintTheBoxWithTheRules_ShouldValidateIfBoxWasPrintedOnce()
    {
        // Arrange
        var builder = new RulesBox(_fixture.WorkspaceSyntax.BrConfiguration.Rules);

        var command = new ListRulesCommand(_fixture.WorkspaceSyntax.BrConfiguration, _printBoxMock.Object);

        // Act
        await command.ExecuteAsync(It.IsAny<CancellationToken>());

        // Assert
        _printBoxMock.Verify(c => c.CreateBoxAndPrint(builder), Times.Once);
    }
}
