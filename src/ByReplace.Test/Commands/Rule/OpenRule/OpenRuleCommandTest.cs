using ByReplace.Commands.Rule.OpenRule;
using ByReplace.Printers;
using ByReplace.Test.Analyzers;
using ByReplace.Test.ClassFixture;
using ByReplace.Test.Common.ConfigMock;
using ByReplace.Test.Common.FolderMock;
using Moq;
using Xunit;

namespace ByReplace.Test.Commands.Rule.OpenRule;

public class OpenRuleCommandTest : IClassFixture<WorkspaceFixture<OpenRuleCommandTest>>
{
    private readonly WorkspaceFixture<OpenRuleCommandTest> _fixture;
    private readonly Mock<IPrint> _printMock;
    private readonly Mock<IPrintBox> _printBoxMock;

    public OpenRuleCommandTest(WorkspaceFixture<OpenRuleCommandTest> fixture)
    {
        _fixture = fixture;
        _printMock = new Mock<IPrint>();
        _printBoxMock = new Mock<IPrintBox>();

        _fixture.ClearPrevious();

        _fixture.WorkspaceSyntax = new WorkspaceSyntax(nameof(OpenRuleCommandTest))
           .BRContent(c =>
           {
               c.AddPath("")
                .AddSkip("obj", ".bin")
                .AddRules(
                   ruleOne => ruleOne
                              .WithName("RuleOne")
                              .WithExtensions(".cs", ".txt")
                              .WithSkips("**\\Controllers\\*", "bin\\bin1.txt", "obj\\obj2.txt")
                              .WithReplacement(BrContentFactory.Replacement("OldText", "NewText")),
                   ruleTwo => ruleTwo
                              .WithName("RuleTwo")
                              .WithExtensions(".cs")
                              .WithSkips("**\\Controllers\\*", "bin\\bin1.txt", "obj\\obj2.txt")
                              .WithReplacement(BrContentFactory.Replacement("MyOldText", "MyNewText")));
           })
           .Folder(folderStructure =>
           {
               folderStructure
                   .AddFile(FileSyntax.FileDeclaration("RootFile1.cs", "ITest = new Test()"))
                   .AddFile(FileSyntax.FileDeclaration("RootFile2.cs", "ITest = new Test()"));
           })
           .Create();
    }

    [Fact]
    public async Task Execute_WhenNotFindTheRuleOnRulesConfiguration_ShouldValidateTheLogThatShowRuleWasNotFind()
    {
        // Arrange
        var command = new OpenRuleCommand(_fixture.WorkspaceSyntax.BrConfiguration, "NotConfiguratedRule", _printMock.Object, _printBoxMock.Object);

        // Act
        await command.ExecuteAsync(It.IsAny<CancellationToken>());

        // Assert
        _printMock.Verify(c => c.Warning("Rule named NotConfiguratedRule was not found on brconfig file"));
    }

    [Fact]
    public async Task Execute_WhenFindTheRuleOnRulesConfiguration_ShouldValidateIfThePrintBoxWasCalled()
    {
        // Arrange
        var expectedRule = _fixture.WorkspaceSyntax.BrConfiguration.Rules.Last();
        var expectedPrintBox = new RuleBox(expectedRule);
        var command = new OpenRuleCommand(_fixture.WorkspaceSyntax.BrConfiguration, "RuleTwo", _printMock.Object, _printBoxMock.Object);

        // Act
        await command.ExecuteAsync(It.IsAny<CancellationToken>());

        // Assert
        _printBoxMock.Verify(c => c.CreateBoxAndPrint(expectedPrintBox), Times.Once);
    }
}
