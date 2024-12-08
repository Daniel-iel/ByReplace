using ByReplace.Printers;
using ByReplace.Providers;
using ByReplace.Test.TestHelpers.ClassFixture;
using ByReplace.Test.TestHelpers.FolderMock;
using Moq;
using Xunit;

namespace ByReplace.Test.Providers;

public class SourceThreeProviderTest : IClassFixture<WorkspaceFixture<SourceThreeProviderTest>>
{
    private readonly WorkspaceFixture<SourceThreeProviderTest> _fixture;
    private readonly Mock<IPrint> _printMock;

    public SourceThreeProviderTest(WorkspaceFixture<SourceThreeProviderTest> fixture)
    {
        _fixture = fixture;
        _printMock = new Mock<IPrint>();

        _fixture.ClearPrevious();

        _fixture.WorkspaceSyntax = new WorkspaceSyntax(nameof(SourceThreeProviderTest))
            .Folder(folderStructure =>
            {
                folderStructure
                    .AddFile(FileSyntax.FileDeclaration("FileOne.cs", "ITest = new Test()"))
                    .AddFolder("FirstLevel", c =>
                    {
                        c.AddFile(FileSyntax.FileDeclaration("FileSecond.cs", "ITest2 = new Test()"));
                    });
            })
            .Create();
    }

    //[Fact]
    //public void LoadThreeFiles_MapAllSourceThreeOfDirectory_ShouldReturnSourceFileThree()
    //{
    //    // Arrange
    //    var analyzer = new SourceThreeProvider(_fixture.WorkspaceSyntax.BrConfiguration, _printMock.Object);

    //    // Act
    //    var directoryNodes = analyzer.Run();

    //    // Assert
    //    Assert.Equal(3, directoryNodes.Count);
    //    Assert.Collection(directoryNodes,
    //          node => Assert.Single(node.Files),
    //          node => Assert.Single(node.Files),
    //          node => Assert.Single(node.Files));
    //}

    [Fact]
    public void LoadThreeFiles_WhenPrintLogInformation_ShouldValidateLogWasCalled()
    {
        // Arrange
        var analyzer = new SourceThreeProvider(_fixture.WorkspaceSyntax.BrConfiguration, _printMock.Object);

        // Act
        var directoryNodes = analyzer.GetSourceThree();

        // Assert
        _printMock.Verify(x => x.Information("Identifying folder three files."), Times.Once);
    }

}
