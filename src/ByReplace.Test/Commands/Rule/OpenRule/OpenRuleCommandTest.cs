using ByReplace.Builders;
using ByReplace.Commands.Rule.ListRules;
using ByReplace.Commands.Rule.OpenRule;
using ByReplace.Models;
using ByReplace.Printers;
using ByReplace.Test.Analyzers;
using ByReplace.Test.Common.ConfigMock;
using ByReplace.Test.Common.FolderMock;
using Moq;
using System.Data;
using Xunit;

namespace ByReplace.Test.Commands.Rule.OpenRule;

public class OpenRuleCommandTest
{
    private readonly PathCompilationSyntax _pathCompilationSyntax;
    private readonly BrConfiguration _brConfiguration;
    private readonly Mock<IPrint> _printMock;
    private readonly Mock<IPrintBox> _printBoxMock;

    public OpenRuleCommandTest()
    {
        _printMock = new Mock<IPrint>();
        _printBoxMock = new Mock<IPrintBox>();

        var configContent = BrContentFactory
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
            .AddMembers(
                FileSyntax.FileDeclaration("RootFile1.cs", "ITest = new Test()"),
                FileSyntax.FileDeclaration("RootFile2.cs", "ITest = new Test()"));

        _pathCompilationSyntax = PathFactory
            .Compile(nameof(AnalyzerAndFixerTest))
            .AddMembers(rootFolder)
            .AddBrConfiguration(configContent)
            .Create();

        _brConfiguration = BrConfigurationBuilder
        .Create()
            .SetPath($"./{_pathCompilationSyntax.InternalIdentifier}")
            .SetConfigPath($"./{_pathCompilationSyntax.InternalIdentifier}")
            .Build();
    }

    [Fact]
    public async Task Execute_WhenNotFindTheRuleOnRulesConfiguration_ShouldValidateTheLogThatShowRuleWasNotFind()
    {
        // Arrange
        var command = new OpenRuleCommand(_brConfiguration, "NotConfiguratedRule", _printMock.Object, _printBoxMock.Object);

        // Act
        await command.ExecuteAsync(It.IsAny<CancellationToken>());

        // Assert
        _printMock.Verify(c => c.Warning("Rule named NotConfiguratedRule was not found on brconfig file"));
    }

    [Fact]
    public async Task Execute_WhenFindTheRuleOnRulesConfiguration_ShouldValidateIfThePrintBoxWasCalled()
    {
        // Arrange
        var expectedRule = _brConfiguration.Rules.Last();
        var expectedPrintBox = new RuleBox(expectedRule);
        var command = new OpenRuleCommand(_brConfiguration, "RuleTwo", _printMock.Object, _printBoxMock.Object);

        // Act
        await command.ExecuteAsync(It.IsAny<CancellationToken>());

        // Assert
        _printBoxMock.Verify(c => c.CreateBoxAndPrint(expectedPrintBox), Times.Once);
    }
}
