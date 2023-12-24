using ByReplace.Analyzers;
using ByReplace.Builders;
using ByReplace.Models;
using ByReplace.Printers;
using ByReplace.Test.Common.ConfigMock;
using ByReplace.Test.Common.FolderMock;
using Moq;
using Xunit;

namespace ByReplace.Test.Analyzers;

public class AnalyzerRunnerTest
{
    private readonly PathCompilationSyntax _pathCompilationSyntax;
    private readonly BrConfiguration _brConfiguration;
    private readonly Mock<IPrint> _printMock;

    public AnalyzerRunnerTest()
    {
        var configContent = BrContentFactory
           .CreateDefault()
           .AddConfig(BrContentFactory.ConfigNoPathDeclaration("obj", ".bin"))
           .AddRules(BrContentFactory
                    .Rule("RuleTest")
                    .WithExtensions(".cs")
                    .WithSkips("**/Controller/*")
                    .WithReplacement(BrContentFactory.Replacement("Test", "Test2")))
           .Compile();

        var rootFolder = FolderSyntax
            .FolderDeclaration("RootFolder")
            .AddMembers(
                FileSyntax.FileDeclaration("RootFile1.cs", "ITest = new Test()"),
                FileSyntax.FileDeclaration("RootFile1.cs", "ITest = new Test()"));

        var controllerFolder = FolderSyntax.FolderDeclaration("Controller")
            .AddParent(rootFolder)
            .AddMembers(
               FileSyntax.FileDeclaration("Controller1.cs", "ITest2 = new Test()"),
               FileSyntax.FileDeclaration("Controller2.cs", "ITest2 = new Test()"));

        _pathCompilationSyntax = PathFactory
            .Compile(nameof(AnalyzersTest))
            .AddMembers(controllerFolder)
            .AddBrConfiguration(configContent)
            .Create();

        _brConfiguration = BrConfigurationBuilder
            .Create()
            .SetPath($"./{_pathCompilationSyntax.InternalIdentifier}")
            .SetConfigPath($"./{_pathCompilationSyntax.InternalIdentifier}")
            .Build();

        _printMock = new Mock<IPrint>();
    }

    [Fact]
    public void RunAnalysis_MapAllRulesThatMatchToFileInSourceTree_ShouldReturnRulesThatMatch()
    {
        // Arrange
        var analyzer = new Analyzer(_brConfiguration, _printMock.Object);
        var analyzerRunner = new AnalyzerRunner(_brConfiguration, _printMock.Object);

        // Act
        var directoryNodes = analyzer.LoadThreeFiles();
        analyzerRunner.RunAnalysis(directoryNodes, Analyses.Fix);

        // Assert
        Assert.Fail();

    }

    [Fact]
    public void RunAnalysis_WhenPrintLogInformation_ShouldValidateLogWasCalled()
    {
        // Arrange
        var analyzer = new Analyzer(_brConfiguration, _printMock.Object);
        var analyzerRunner = new AnalyzerRunner(_brConfiguration, _printMock.Object);

        // Act
        var directoryNodes = analyzer.LoadThreeFiles();
        analyzerRunner.RunAnalysis(directoryNodes, Analyses.Fix);

        // Assert
        _printMock.Verify(x => x.Information("Identifying rules that has matches."), Times.Once);
    }
}
