using ByReplace.Analyzers;
using ByReplace.Printers;
using ByReplace.Test.ClassFixture;
using ByReplace.Test.Common.FolderMock;
using Moq;
using Xunit;

namespace ByReplace.Test.Analyzers;

public class AnalyzerTest : IClassFixture<WorkspaceFixture<AnalyzerTest>>
{
    private readonly WorkspaceFixture<AnalyzerTest> _fixture;
    private readonly Mock<IPrint> _printMock;

    public AnalyzerTest(WorkspaceFixture<AnalyzerTest> fixture)
    {
        _fixture = fixture;
        _printMock = new Mock<IPrint>();

        _fixture.ClearPrevious();

        _fixture.WorkspaceSyntax = new WorkspaceSyntax(nameof(AnalyzerTest))
            .Folder(folderStructure =>
            {
                folderStructure
                    .AddFile(FileSyntaxV2.FileDeclaration("FileOne.cs", "ITest = new Test()"))
                    .AddFolder("FirstLevel", c =>
                    {
                        c.AddFile(FileSyntaxV2.FileDeclaration("FileSecond.cs", "ITest2 = new Test()"));
                    });
            })
            .Create();
    }

    [Fact]
    public void LoadThreeFiles_MapAllSourceThreeOfDirectory_ShouldReturnSourceFileThree()
    {
        // Arrange
        var analyzer = new Analyzer(_fixture.WorkspaceSyntax.BrConfiguration, _printMock.Object);

        // Act
        var directoryNodes = analyzer.LoadThreeFiles();

        // Assert
        Assert.Equal(3, directoryNodes.Count);
        Assert.Collection(directoryNodes,
              node => Assert.Single(node.Files),
              node => Assert.Single(node.Files),
              node => Assert.Single(node.Files));
    }

    [Fact]
    public void LoadThreeFiles_WhenPrintLogInformation_ShouldValidateLogWasCalled()
    {
        // Arrange
        var analyzer = new Analyzer(_fixture.WorkspaceSyntax.BrConfiguration, _printMock.Object);

        // Act
        var directoryNodes = analyzer.LoadThreeFiles();

        // Assert
        _printMock.Verify(x => x.Information("Identifying folder three files."), Times.Once);
    }
}
