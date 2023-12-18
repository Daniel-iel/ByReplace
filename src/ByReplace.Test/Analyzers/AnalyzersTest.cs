using ByReplace.Analyzers;
using ByReplace.Builders;
using ByReplace.Models;
using ByReplace.Printers;
using ByReplace.Test.Common.FolderMock;
using Moq;
using Xunit;

namespace ByReplace.Test.Analyzers;

public class AnalyzersTest
{
    private readonly PathCompilationSyntax _pathCompilationSyntax;
    private readonly BrConfiguration _brConfiguration;
    private readonly Mock<IPrint> _printMock;

    public AnalyzersTest()
    {
        var levelOne = FolderSyntax
           .FolderDeclaration("OneLevel")
           .AddMenbers(FileSyntax.FileDeclaration("FileOne.cs", "ITest = new Test()"));

        var SecondOne = FolderSyntax.FolderDeclaration("SecondLevel")
            .AddParent(levelOne)
            .AddMenbers(FileSyntax.FileDeclaration("FileSecond.cs", "ITest2 = new Test()"));

        _pathCompilationSyntax = PathFactory
           .Compile(nameof(AnalyzersTest))
           .AddFolders(SecondOne)
           .CreateThreeFolder();

        _brConfiguration = BrConfigurationBuilder
        .Instantiate()
           .SetPath($"./{_pathCompilationSyntax.InternalIdentifier}")
           .SetConfigPath($"./{_pathCompilationSyntax.InternalIdentifier}")
           .Build();

        _printMock = new Mock<IPrint>();
    }

    [Fact]
    public void LoadThreeFiles_MapAllSourceThreeOfDirectory_ShouldReturnSourceFileThree()
    {
        // Arrange
        var analyzer = new Analyzer(_brConfiguration, _printMock.Object);

        // Act
        var directoryNodes = analyzer.LoadThreeFiles();

        // Assert
        Assert.Equal(3, directoryNodes.Count);
        Assert.Single(directoryNodes[0].Files);
        Assert.Single(directoryNodes[1].Files);
        Assert.Single(directoryNodes[2].Files);
    }

    [Fact]
    public void LoadThreeFiles_WhenPrintLogInformation_ShouldValidateLogWasCalled()
    {
        // Arrange
        var analyzer = new Analyzer(_brConfiguration, _printMock.Object);

        // Act
        var directoryNodes = analyzer.LoadThreeFiles();

        // Assert
        _printMock.Verify(x => x.Information("Identifying folder three files."), Times.Once);
    }
}
