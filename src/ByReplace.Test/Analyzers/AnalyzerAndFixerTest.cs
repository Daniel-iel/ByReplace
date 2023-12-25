using ByReplace.Analyzers;
using ByReplace.Builders;
using ByReplace.Models;
using ByReplace.Printers;
using ByReplace.Test.Common.ConfigMock;
using ByReplace.Test.Common.FolderMock;
using Moq;
using Xunit;

namespace ByReplace.Test.Analyzers;

public class AnalyzerAndFixerTest
{
    private readonly PathCompilationSyntax _pathCompilationSyntax;
    private readonly BrConfiguration _brConfiguration;
    private readonly Mock<IPrint> _printMock;

    public AnalyzerAndFixerTest()
    {
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
            .Compile(nameof(AnalyzerTest))
            .AddMembers(rootFolder)
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
    public void TryMatchRule_MapTheFilesThatMatchToRule_ShouldReturnFilesThatMatch()
    {
        // Arrange
        var analyzer = new Analyzer(_brConfiguration, _printMock.Object);
        var analyzerAndFixer = new AnalyzerAndFixer(_printMock.Object);

        // Act
        var directoryNode = analyzer.LoadThreeFiles().Last();
        analyzerAndFixer.TryMatchRule(directoryNode, _brConfiguration.Rules);

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
        var analyzer = new Analyzer(_brConfiguration, _printMock.Object);
        var analyzerAndFixer = new AnalyzerAndFixer(_printMock.Object);

        // Act
        var directoryNode = analyzer.LoadThreeFiles().Last();
        analyzerAndFixer.TryMatchRule(directoryNode, _brConfiguration.Rules);

        // Assert
        _printMock.Verify(x => x.Information("[Cyan]1 rules in total match the file [Cyan]RootFile1.cs."), Times.Once);
        _printMock.Verify(x => x.Information("[Cyan]1 rules in total match the file [Cyan]RootFile2.cs."), Times.Once);
    }
}
