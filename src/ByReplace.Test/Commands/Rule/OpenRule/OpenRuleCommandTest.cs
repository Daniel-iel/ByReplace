using ByReplace.Builders;
using ByReplace.Commands.Rule.OpenRule;
using ByReplace.Printers;
using ByReplace.Test.ClassFixture;
using ByReplace.Test.Common.ConfigMock;
using ByReplace.Test.Common.FolderMock;
using Moq;
using Xunit;

namespace ByReplace.Test.Commands.Rule.OpenRule;

public class OpenRuleCommandTest : IClassFixture<WorkspaceFixture>
{
    private readonly WorkspaceFixture _workspace;
    private readonly Mock<IPrint> _printMock;
    private readonly Mock<IPrintBox> _printBoxMock;

    public OpenRuleCommandTest(WorkspaceFixture workspace)
    {
        _workspace = workspace;
        _printMock = new Mock<IPrint>();
        _printBoxMock = new Mock<IPrintBox>();

        _workspace.ClearPrevious();

        workspace.ConfigContent = BrContentFactory
            .CreateDefault()
            .AddConfig(BrContentFactory.ConfigNoPathDeclaration("obj", ".bin"))
            .AddRules(
                BrContentFactory
                .Rule("RuleOne")
                .WithExtensions(".cs", ".txt")
                .WithSkips("**\\Controllers\\*", "bin\\bin1.txt", "obj\\obj2.txt")
                .WithReplacement(BrContentFactory.Replacement("OldText", "NewText")),
                BrContentFactory
                .Rule("RuleTwo")
                .WithExtensions(".cs")
                .WithSkips("**\\Controllers\\*", "bin\\bin1.txt", "obj\\obj2.txt")
                .WithReplacement(BrContentFactory.Replacement("MyOldText", "MyNewText"))
              )
            .Compile();

        var rootFolder = FolderSyntax
            .FolderDeclaration("RootFolder")
            .AddFiles(
                FileSyntax.FileDeclaration("RootFile1.cs", "ITest = new Test()"),
                FileSyntax.FileDeclaration("RootFile2.cs", "ITest = new Test()"));

        workspace.WorkspaceSyntax = WorkspaceFactory
            .Compile(nameof(OpenRuleCommandTest))
            .AddMembers(rootFolder)
            .AddBrConfiguration(workspace.ConfigContent)
            .Create();

        workspace.BrConfiguration = BrConfigurationBuilder
            .Create()
            .SetPath($"./{workspace.WorkspaceSyntax.Identifier}")
            .SetConfigPath($"./{workspace.WorkspaceSyntax.Identifier}")
            .Build();
    }

    [Fact]
    public async Task Execute_WhenNotFindTheRuleOnRulesConfiguration_ShouldValidateTheLogThatShowRuleWasNotFind()
    {
        // Arrange
        var command = new OpenRuleCommand(_workspace.BrConfiguration, "NotConfiguratedRule", _printMock.Object, _printBoxMock.Object);

        // Act
        await command.ExecuteAsync(It.IsAny<CancellationToken>());

        // Assert
        _printMock.Verify(c => c.Warning("Rule named NotConfiguratedRule was not found on brconfig file"));
    }

    [Fact]
    public async Task Execute_WhenFindTheRuleOnRulesConfiguration_ShouldValidateIfThePrintBoxWasCalled()
    {
        // Arrange
        var expectedRule = _workspace.BrConfiguration.Rules.Last();
        var expectedPrintBox = new RuleBox(expectedRule);
        var command = new OpenRuleCommand(_workspace.BrConfiguration, "RuleTwo", _printMock.Object, _printBoxMock.Object);

        // Act
        await command.ExecuteAsync(It.IsAny<CancellationToken>());

        // Assert
        _printBoxMock.Verify(c => c.CreateBoxAndPrint(expectedPrintBox), Times.Once);
    }
}
