using ByReplace.Analyzers;
using ByReplace.Printers;
using ByReplace.Test.ClassFixture;
using Moq;
using Xunit;

namespace ByReplace.Test.Analyzers;

public class AnalyzerAndFixerTest : IClassFixture<WorkspaceFixture>
{
    private readonly WorkspaceFixture _workspace;
    private readonly Mock<IPrint> _printMock;

    public AnalyzerAndFixerTest(WorkspaceFixture workspace)
    {
        _workspace = workspace;
        _printMock = new Mock<IPrint>();
    }

    [Fact]
    public void TryMatchRule_MapTheFilesThatMatchToRule_ShouldReturnFilesThatMatch()
    {
        // Arrange
        var analyzer = new Analyzer(_workspace.BrConfiguration, _printMock.Object);
        var analyzerAndFixer = new AnalyzerAndFixer(_printMock.Object);

        // Act
        var directoryNode = analyzer.LoadThreeFiles().Last();
        analyzerAndFixer.TryMatchRule(directoryNode, _workspace.BrConfiguration.Rules);

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
        var analyzer = new Analyzer(_workspace.BrConfiguration, _printMock.Object);
        var analyzerAndFixer = new AnalyzerAndFixer(_printMock.Object);

        // Act
        var directoryNode = analyzer.LoadThreeFiles().Last();
        analyzerAndFixer.TryMatchRule(directoryNode, _workspace.BrConfiguration.Rules);

        // Assert
        _printMock.Verify(x => x.Information("[Cyan]1 rules in total match the file [Cyan]RootFile1.cs."), Times.Once);
        _printMock.Verify(x => x.Information("[Cyan]1 rules in total match the file [Cyan]RootFile2.cs."), Times.Once);
    }
}