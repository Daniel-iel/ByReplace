﻿using ByReplace.Analyzers;
using ByReplace.Builders;
using ByReplace.Models;
using ByReplace.Printers;
using ByReplace.Test.Common.ConfigMock;
using ByReplace.Test.Common.FolderMock;
using Moq;
using Xunit;

namespace ByReplace.Test.Analyzers;

public class DocumentFixTest
{
    private readonly PathCompilationSyntax _pathCompilationSyntax;
    private readonly BrConfiguration _brConfiguration;
    private readonly Mock<IPrint> _printMock;

    public DocumentFixTest()
    {
        var configContent = BrContentFactory
          .CreateDefault()
          .AddConfig(BrContentFactory.ConfigNoPathDeclaration("obj", ".bin"))
          .AddRules(BrContentFactory
                   .Rule("RuleTest")
                   .WithExtensions(".cs")
                   .WithSkips("teste.cs")
                   .WithReplacement(BrContentFactory.Replacement("Test", "Test2")),
                   BrContentFactory
                   .Rule("RuleTest2")
                   .WithExtensions(".txt")
                   .WithSkips("teste.txt")
                   .WithReplacement(BrContentFactory.Replacement("Test", "Test2")))
          .Compile();

        var rootFolder = FolderSyntax
            .FolderDeclaration("RootFolder")
            .AddMembers(
            FileSyntax.FileDeclaration("RootFile1.cs", "var test = new Test2()"),
            FileSyntax.FileDeclaration("RootFile1.txt", "var test = new Test2()"));

        _pathCompilationSyntax = PathFactory
            .Compile(nameof(DocumentFixTest))
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
    public async Task ApplyAsync_WhenPassAllRules_ShouldApplyTheRulesInAllFilesAsync()
    {
        // Arrange
        var analyzer = new Analyzer(_brConfiguration, _printMock.Object);
        var analyzerAndFixer = new AnalyzerAndFixer(_printMock.Object);
        var directoryNode = analyzer.LoadThreeFiles().Last();
        analyzerAndFixer.TryMatchRule(directoryNode, _brConfiguration.Rules);
        var documentFix = new DocumentFix(analyzerAndFixer, _printMock.Object);

        // Act
        await documentFix.ApplyAsync(It.IsAny<CancellationToken>());

        // Assert
        var fileFixedPath = directoryNode.Files[0].FullName;
        var fileContents = await File.ReadAllTextAsync(fileFixedPath, It.IsAny<CancellationToken>());

        Assert.Contains("var test = new Test()", fileContents);
        _printMock.Verify(x => x.Information("Initializing fixing."), Times.Once);
        _printMock.Verify(x => x.Information("Processing file [Cyan]RootFile1.cs"), Times.Once);
        _printMock.Verify(x => x.Information("Applying rule [Cyan]RuleTest 1/1 on file [Cyan]RootFile1.cs."), Times.Once);
    }

    [Fact]
    public async Task ApplyAsync_WhenPassOnlyOneRule_ShouldApplyTheRuleInAllFiles()
    {
        // Arrange
        var analyzer = new Analyzer(_brConfiguration, _printMock.Object);
        var analyzerAndFixer = new AnalyzerAndFixer(_printMock.Object);
        var directoryNode = analyzer.LoadThreeFiles().Last();
        analyzerAndFixer.TryMatchRule(directoryNode, _brConfiguration.Rules);
        var documentFix = new DocumentFix(analyzerAndFixer, _printMock.Object);

        // Act
        await documentFix.ApplyAsync("RuleTest", It.IsAny<CancellationToken>());

        // Assert

        // File with .cs extension
        var fileCsFixedPath = directoryNode.Files[0].FullName;
        var fileCsContents = await File.ReadAllTextAsync(fileCsFixedPath, It.IsAny<CancellationToken>());

        // File with .txt extension
        var fileTextFixedPath = directoryNode.Files.Last().FullName;
        var fileTextContents = await File.ReadAllTextAsync(fileTextFixedPath, It.IsAny<CancellationToken>());

        Assert.Contains("var test = new Test()", fileCsContents);
        Assert.Contains("var test = new Test2()", fileTextContents);

        _printMock.Verify(x => x.Information("Initializing fixing."), Times.Once);
        _printMock.Verify(x => x.Information("Processing file [Cyan]RootFile1.cs"), Times.Once);
        _printMock.Verify(x => x.Information("Applying rule [Cyan]RuleTest 1/1 on file [Cyan]RootFile1.cs."), Times.Once);
    }
}
