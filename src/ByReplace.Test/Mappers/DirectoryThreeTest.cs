using ByReplace.Builders;
using ByReplace.Mappers;
using ByReplace.Models;
using ByReplace.Printers;
using ByReplace.Test.Common.ConfigMock;
using ByReplace.Test.Common.FolderMock;
using Moq;
using Xunit;

namespace ByReplace.Test.Mappers;

public class DirectoryThreeTest
{
    private readonly WorkspaceSyntax _workspaceSyntax;
    private readonly BrConfiguration _brConfiguration;
    private readonly Mock<IPrint> _printMock;

    public DirectoryThreeTest()
    {
        _printMock = new Mock<IPrint>();

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
            .AddFiles(
                FileSyntax.FileDeclaration("RootFile1.cs", "ITest = new Test()"),
                FileSyntax.FileDeclaration("RootFile2.cs", "ITest = new Test()"));

        _workspaceSyntax = WorkspaceFactory
            .Compile(nameof(DirectoryThreeTest))
            .AddMembers(rootFolder)
            .AddBrConfiguration(configContent)
            .Create();

        _brConfiguration = BrConfigurationBuilder
            .Create()
            .SetPath($"./{_workspaceSyntax.Identifier}")
            .SetConfigPath($"./{_workspaceSyntax.Identifier}")
            .Build();
    }

    [Fact]
    public void MapThreeSources_WhenStartTheSourceMap_ShouldReturnTheThreeFile()
    {
        // Arrange
        var dirThree = new DirectoryThree(_printMock.Object);

        // Act
        var nodes = dirThree.MapThreeSources(_brConfiguration.Path);

        // Assert
        Assert.Equal(2, nodes.Count);
        Assert.Collection(nodes,
        entry =>
        {
            Assert.Single(entry.Files);
            Assert.NotEmpty(entry.Parent);
            Assert.NotEmpty(entry.Path);
            Assert.Collection(entry.Files,
            entry =>
            {
                Assert.Equal(".json", entry.Extension);
                Assert.EndsWith("brconfig.json", entry.FullName);
                Assert.Equal("brconfig.json", entry.Name);
            });
        },
        entry =>
        {
            Assert.Equal(2, entry.Files.Count);
            Assert.NotEmpty(entry.Parent);
            Assert.NotEmpty(entry.Path);
            Assert.Collection(entry.Files,
            entry =>
            {
                Assert.Equal(".cs", entry.Extension);
                Assert.EndsWith("RootFile1.cs", entry.FullName);
                Assert.Equal("RootFile1.cs", entry.Name);
            },
            entry =>
            {
                Assert.Equal(".cs", entry.Extension);
                Assert.EndsWith("RootFile2.cs", entry.FullName);
                Assert.Equal("RootFile2.cs", entry.Name);
            });
        });
    }
}
