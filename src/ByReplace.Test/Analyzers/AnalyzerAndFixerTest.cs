using ByReplace.Analyzers;
using ByReplace.Printers;
using ByReplace.Providers;
using ByReplace.Test.TestHelpers.ClassFixture;
using ByReplace.Test.TestHelpers.ConfigMock;
using ByReplace.Test.TestHelpers.FolderMock;
using Moq;
using Xunit;

namespace ByReplace.Test.Analyzers;

public class AnalyzerAndFixerTest : IClassFixture<WorkspaceFixture<AnalyzerAndFixerTest>>
{
    private readonly WorkspaceFixture<AnalyzerAndFixerTest> _fixture;
    private readonly Mock<IPrint> _printMock;

    public AnalyzerAndFixerTest(WorkspaceFixture<AnalyzerAndFixerTest> fixture)
    {
        _printMock = new Mock<IPrint>();
        _fixture = fixture;
        _fixture.ClearPrevious();

        _fixture.WorkspaceSyntax = new WorkspaceSyntax(typeof(AnalyzerAndFixerTest).Name)
            .BRContent(c =>
            {
                c.AddPath("")
                .AddSkip("obj", ".bin")
                .AddRules(ruleOnde => ruleOnde
                            .WithName("RuleOne")
                            .WithExtensions(".json", ".rar")
                            .WithSkips("brconfig.json")
                            .WithReplacement(BrContentFactory.Replacement("Test", "Test2")),
                            ruleTwo => ruleTwo
                            .WithName("RuleTwo")
                            .WithExtensions(".cs", ".txt")
                            .WithSkips("**\\Controllers\\*", "bin\\bin1.txt", "obj\\obj2.txt")
                            .WithReplacement(BrContentFactory.Replacement("Test", "Test2")));
            })
            .Folder(folderStructure =>
            {
                folderStructure
                    .AddFile(FileSyntax.FileDeclaration("RootFile1.cs", "ITest = new Test2()", folderStructure.Name))
                    .AddFile(FileSyntax.FileDeclaration("RootFile2.cs", "ITest = new Test2()", folderStructure.Name));
            })
            .Create();
    }

    [Fact]
    public void TryMatchRule_MapTheFilesThatMatchToRule_ShouldReturnFilesThatMatch()
    {
        // Arrange
        var analyzer = new SourceThreeProvider(_fixture.WorkspaceSyntax.BrConfiguration, _printMock.Object);
        var analyzerAndFixer = new AnalyzerAndFixer(_printMock.Object, _fixture.WorkspaceSyntax.BrConfiguration.Rules);

        // Act
        var directoryNode = analyzer.GetSourceThree().Last();
        analyzerAndFixer.TryMatchRule(directoryNode);

        // Assert
        Assert.Equal(2, analyzerAndFixer.Count);

        Assert.Collection(analyzerAndFixer,
        entry =>
        {
            var file = new FileInfo(entry.Key.Path);
            Assert.Equal("RootFile1.cs", file.Name);
            Assert.Equal(".cs", file.Extension);
            Assert.Collection(entry.Value, rule => Assert.Equal("RuleTest", rule.Name));
        },
        entry =>
        {
            var file = new FileInfo(entry.Key.Path);
            Assert.Equal("RootFile2.cs", file.Name);
            Assert.Equal(".cs", file.Extension);
            Assert.Collection(entry.Value, rule => Assert.Equal("RuleTest", rule.Name));
        });
    }

    [Fact]
    public void TryMatchRule_WhenMapTheFilesThatMatchToRule_ShouldValidateLogWasCalled()
    {
        // Arrange
        var analyzer = new SourceThreeProvider(_fixture.WorkspaceSyntax.BrConfiguration, _printMock.Object);
        var analyzerAndFixer = new AnalyzerAndFixer(_printMock.Object, _fixture.WorkspaceSyntax.BrConfiguration.Rules);

        // Act
        var directoryNode = analyzer.GetSourceThree().Last();
        analyzerAndFixer.TryMatchRule(directoryNode);

        // Assert

        //"[Cyan]{0} rules in total match the file [Cyan]{1}.", item.Value.Count, item.Key.Name
        _printMock.Verify(x => x.Information("[Cyan]1 rules in total match the file [Cyan]RootFile1.cs."), Times.Once);
        _printMock.Verify(x => x.Information("[Cyan]1 rules in total match the file [Cyan]RootFile2.cs."), Times.Once);
    }

    [Fact]
    public void FindByRule_ShouldMatchOnlyFilteredRules_WhenSourceProviderHasMultipleFiles()
    {
        // Arrange
        var sourceThreeProvider = new SourceThreeProvider(_fixture.WorkspaceSyntax.BrConfiguration, _printMock.Object);
        var matchProvider = new MatchProvider(_fixture.WorkspaceSyntax.BrConfiguration, _printMock.Object, sourceThreeProvider);

        // Act
        var analyzerAndFixer = matchProvider.Run();
        var analyzerAndFixerFiltred = analyzerAndFixer.FindByRule("RuleOne");

        // Assert
        _printMock.Verify(x => x.Information("[Cyan]1 rules in total match the file [Cyan]RootFile1.cs."), Times.Once);
        _printMock.Verify(x => x.Information("[Cyan]1 rules in total match the file [Cyan]RootFile2.cs."), Times.Once);
    }
}
