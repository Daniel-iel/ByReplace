using ByReplace.Commands.Rule.ListRules;
using ByReplace.Printers;
using ByReplace.Test.ClassFixture;
using Moq;
using Xunit;

namespace ByReplace.Test.Commands.Rule.ListRules;

public class ListRulesCommandTest : IClassFixture<WorkspaceFixture>
{
    private readonly WorkspaceFixture _workspace;
    private readonly Mock<IPrintBox> _printBoxMock;

    public ListRulesCommandTest(WorkspaceFixture workspace)
    {
        _workspace = workspace;
        _printBoxMock = new Mock<IPrintBox>();
    }

    [Fact]
    public async Task Execute_WhenPrintTheBoxWithTheRules_ShouldValidateIfBoxWasPrintedOnce()
    {
        // Arrange
        var builder = new RulesBox(_workspace.BrConfiguration.Rules);

        var command = new ListRulesCommand(_workspace.BrConfiguration, _printBoxMock.Object);

        // Act
        await command.ExecuteAsync(It.IsAny<CancellationToken>());

        // Assert
        _printBoxMock.Verify(c => c.CreateBoxAndPrint(builder), Times.Once);
    }
}
