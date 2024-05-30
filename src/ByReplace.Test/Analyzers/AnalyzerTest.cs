using ByReplace.Analyzers;
using ByReplace.Builders;
using ByReplace.Printers;
using ByReplace.Test.ClassFixture;
using ByReplace.Test.Common.FolderMock;
using Moq;
using Xunit;

namespace ByReplace.Test.Analyzers;

public class AnalyzerTest : IClassFixture<WorkspaceFixture>
{
    private readonly WorkspaceFixture _workspace;
    private readonly Mock<IPrint> _printMock;

    public AnalyzerTest(WorkspaceFixture workspace)
    {
        _workspace = workspace;
        _printMock = new Mock<IPrint>();

        _workspace.ClearPrevious();

        var rootFolder = FolderSyntax
           .FolderDeclaration("RootFolder")
           .AddMembers(FileSyntax.FileDeclaration("FileOne.cs", "ITest = new Test()"));

        var firstLevel = FolderSyntax.FolderDeclaration("FirstLevel")
           .AddParent(rootFolder)
           .AddMembers(FileSyntax.FileDeclaration("FileSecond.cs", "ITest2 = new Test()"));

        _workspace.WorkspaceSyntax = WorkspaceFactory
           .Compile(nameof(AnalyzerTest))
           .AddMembers(firstLevel)
           .Create();

        _workspace.BrConfiguration = BrConfigurationBuilder
           .Create()
           .SetPath($"./{_workspace.WorkspaceSyntax.Identifier}")
           .SetConfigPath($"./{_workspace.WorkspaceSyntax.Identifier}")
           .Build();
    }

    [Fact]
    public void LoadThreeFiles_MapAllSourceThreeOfDirectory_ShouldReturnSourceFileThree()
    {
        // Arrange
        var analyzer = new Analyzer(_workspace.BrConfiguration, _printMock.Object);

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
        var analyzer = new Analyzer(_workspace.BrConfiguration, _printMock.Object);

        // Act
        var directoryNodes = analyzer.LoadThreeFiles();

        // Assert
        _printMock.Verify(x => x.Information("Identifying folder three files."), Times.Once);
    }
}
