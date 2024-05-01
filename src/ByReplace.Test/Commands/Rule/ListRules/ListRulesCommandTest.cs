using ByReplace.Builders;
using ByReplace.Commands.Rule.ListRules;
using ByReplace.Models;
using ByReplace.Printers;
using ByReplace.Test.Analyzers;
using ByReplace.Test.Common.ConfigMock;
using ByReplace.Test.Common.FolderMock;
using Moq;
using Xunit;

namespace ByReplace.Test.Commands.Rule.ListRules;

public class ListRulesCommandTest
{
    private readonly PathCompilationSyntax _pathCompilationSyntax;
    private readonly BrConfiguration _brConfiguration;
    private readonly Mock<IPrintBox> _printBoxMock;

    public ListRulesCommandTest()
    {
        _printBoxMock = new Mock<IPrintBox>();

        var configContent = BrContentFactory
            .CreateDefault()
            .AddConfig(BrContentFactory.ConfigNoPathDeclaration("obj", ".bin"))
            .AddRules(BrContentFactory
                     .Rule("RuleTest")
                     .WithExtensions(".cs", ".txt")
                     .WithSkips("**\\Controllers\\*", "bin\\bin1.txt", "obj\\obj2.txt")
                     .WithReplacement(BrContentFactory.Replacement("Test", "Test2")))
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
    public async Task Execute_WhenPrintTheBoxWithTheRules_ShouldValidadeIfBoxWasPrintedOnce()
    {
        // Arrange
        var builder = new RulesBox(_brConfiguration.Rules);

        var command = new ListRulesCommand(_brConfiguration, _printBoxMock.Object);

        // Act
        await command.ExecuteAsync(It.IsAny<CancellationToken>());

        // Assert
        _printBoxMock.Verify(c => c.CreateBoxAndPrint(builder));
    }
}
