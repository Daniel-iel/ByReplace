using ByReplace.Analyzers;
using ByReplace.Printers;
using ByReplace.Test.TestHelpers.ClassFixture;
using Moq;
using Xunit;

namespace ByReplace.Test.Analyzers;

public class AnalyzerAndFixerTest : IClassFixture<WorkspaceFixture<AnalyzerAndFixerTest>>
{
    private readonly WorkspaceFixture<AnalyzerAndFixerTest> _fixture;
    private readonly Mock<IPrint> _printMock;

    public AnalyzerAndFixerTest(WorkspaceFixture<AnalyzerAndFixerTest> fixture)
    {
        _fixture = fixture;
        _printMock = new Mock<IPrint>();
    }

    [Fact]
    public void TryMatchRule_MapTheFilesThatMatchToRule_ShouldReturnFilesThatMatch()
    {
        // Arrange
        var analyzer = new Analyzer(_fixture.WorkspaceSyntax.BrConfiguration, _printMock.Object);
        var analyzerAndFixer = new AnalyzerAndFixer(_printMock.Object, _fixture.WorkspaceSyntax.BrConfiguration.Rules);

        // Act
        var directoryNode = analyzer.LoadThreeFiles().Last();
        analyzerAndFixer.TryMatchRule(directoryNode);

        // Assert
        Assert.Equal(2, analyzerAndFixer.Count);

        Assert.Collection(analyzerAndFixer,
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
        });
    }

    [Fact]
    public void TryMatchRule_WhenMapTheFilesThatMatchToRule_ShouldValidateLogWasCalled()
    {
        // Arrange
        var analyzer = new Analyzer(_fixture.WorkspaceSyntax.BrConfiguration, _printMock.Object);
        var analyzerAndFixer = new AnalyzerAndFixer(_printMock.Object, _fixture.WorkspaceSyntax.BrConfiguration.Rules);

        // Act
        var directoryNode = analyzer.LoadThreeFiles().Last();
        analyzerAndFixer.TryMatchRule(directoryNode);

        // Assert

        //"[Cyan]{0} rules in total match the file [Cyan]{1}.", item.Value.Count, item.Key.Name
        _printMock.Verify(x => x.Information("[Cyan]1 rules in total match the file [Cyan]RootFile1.cs."), Times.Once);
        _printMock.Verify(x => x.Information("[Cyan]1 rules in total match the file [Cyan]RootFile2.cs."), Times.Once);
    }
}
