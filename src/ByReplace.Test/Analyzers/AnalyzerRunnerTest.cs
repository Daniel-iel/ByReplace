using ByReplace.Analyzers;
using ByReplace.Builders;
using ByReplace.Printers;
using ByReplace.Test.ClassFixture;
using ByReplace.Test.Common.ConfigMock;
using ByReplace.Test.Common.FolderMock;
using Moq;
using Xunit;

namespace ByReplace.Test.Analyzers;

public class AnalyzerRunnerTest : IClassFixture<WorkspaceFixture>
{
    private readonly WorkspaceFixture _workspace;
    private readonly Mock<IPrint> _printMock;

    public AnalyzerRunnerTest(WorkspaceFixture workspace)
    {
        _workspace = workspace;
        _printMock = new Mock<IPrint>();

        workspace.ClearPrevious();

        _workspace.ConfigContent = BrContentFactory
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
            .AddFiles(
                FileSyntax.FileDeclaration("RootFile1.cs", "ITest = new Test()"),
                FileSyntax.FileDeclaration("RootFile2.cs", "ITest = new Test()"));

        var controllerFolder = FolderSyntax.FolderDeclaration("Controllers")
            .AddParent(rootFolder)
            .AddFiles(
               FileSyntax.FileDeclaration("Controller1.cs", "ITest2 = new Test()"),
               FileSyntax.FileDeclaration("Controller2.cs", "ITest2 = new Test()"));

        var binFolder = FolderSyntax.FolderDeclaration("bin")
            .AddParent(rootFolder)
            .AddFiles(
                FileSyntax.FileDeclaration("bin1.txt", "ITest = new Test()"),
                FileSyntax.FileDeclaration("bin2.txt", "ITest = new Test()"));

        var objFolder = FolderSyntax.FolderDeclaration("obj")
            .AddParent(rootFolder)
            .AddFiles(
                FileSyntax.FileDeclaration("obj1.txt", "ITest = new Test()"),
                FileSyntax.FileDeclaration("obj2.txt", "ITest = new Test()"));

        _workspace.WorkspaceSyntax = WorkspaceFactory
            .Compile(nameof(AnalyzerRunnerTest))
            .AddMembers(controllerFolder, binFolder, objFolder)
            .AddBrConfiguration(_workspace.ConfigContent)
            .Create();

        _workspace.BrConfiguration = BrConfigurationBuilder
            .Create()
            .SetPath($"./{_workspace.WorkspaceSyntax.Identifier}")
            .SetConfigPath($"./{_workspace.WorkspaceSyntax.Identifier}")
            .Build();
    }

    [Fact]
    public void RunAnalysis_MapAllRulesThatMatchToFileInSourceTree_ShouldReturnRulesThatMatch()
    {
        // Arrange
        var analyzer = new Analyzer(_workspace.BrConfiguration, _printMock.Object);
        var analyzerRunner = new AnalyzerRunner(_workspace.BrConfiguration, _printMock.Object);

        // Act
        var directoryNodes = analyzer.LoadThreeFiles();
        var analyzersAndFixers = analyzerRunner.RunAnalysis(directoryNodes, Analyses.Fix);

        // Assert
        var files = analyzersAndFixers.Keys.ToList();
        var rules = analyzersAndFixers.Values.ToList();

        Assert.Equal(4, analyzersAndFixers.Count);

        Assert.Collection(analyzersAndFixers,
        entry =>
        {
            Assert.Equal("RootFile1.cs", entry.Key.Name);
            Assert.Equal(".cs", entry.Key.Extension);
            Assert.Collection(entry.Value, rule => Assert.Equal("RuleTest", rule.Name));
        },
        entry =>
        {
            Assert.Equal("RootFile2.cs", entry.Key.Name);
            Assert.Equal(".cs", entry.Key.Extension);
            Assert.Collection(entry.Value, rule => Assert.Equal("RuleTest", rule.Name));
        },
        entry =>
        {
            Assert.Equal("bin2.txt", entry.Key.Name);
            Assert.Equal(".txt", entry.Key.Extension);
            Assert.Collection(entry.Value, rule => Assert.Equal("RuleTest", rule.Name));
        },
        entry =>
        {
            Assert.Equal("obj1.txt", entry.Key.Name);
            Assert.Equal(".txt", entry.Key.Extension);
            Assert.Collection(entry.Value, rule => Assert.Equal("RuleTest", rule.Name));
        });
    }

    [Fact]
    public void RunAnalysis_WhenPrintLogInformation_ShouldValidateLogWasCalled()
    {
        // Arrange
        var analyzer = new Analyzer(_workspace.BrConfiguration, _printMock.Object);
        var analyzerRunner = new AnalyzerRunner(_workspace.BrConfiguration, _printMock.Object);

        // Act
        var directoryNodes = analyzer.LoadThreeFiles();
        analyzerRunner.RunAnalysis(directoryNodes, Analyses.Fix);

        // Assert
        _printMock.Verify(x => x.Information("Identifying rules that has matches."), Times.Once);
    }
}
