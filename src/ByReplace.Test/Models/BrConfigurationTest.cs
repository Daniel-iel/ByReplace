using ByReplace.Models;
using ByReplace.Test.Common.ConfigMock;
using ByReplace.Test.Common.FolderMock;
using Xunit;

namespace ByReplace.Test.Models;

public class BrConfigurationTest
{
    private readonly WorkspaceSyntax _workspaceSyntax;

    public BrConfigurationTest()
    {
        _workspaceSyntax = new WorkspaceSyntax(nameof(BrConfigurationTest))
            .BRContent(c =>
            {
                c.AddPath("")
                 .AddSkip("obj", ".bin")
                 .AddRules(
                    ruleOne => ruleOne
                               .WithName("RuleTest")
                               .WithExtensions(".cs", ".txt")
                               .WithSkips("**\\Controllers\\*", "bin\\bin1.txt", "obj\\obj2.txt")
                               .WithReplacement(BrContentFactory.Replacement("Test", "Test2")));
            })
            .Folder(folderStructure =>
            {
                folderStructure
                    .AddFile(FileSyntax.FileDeclaration("RootFile1.cs", "ITest = new Test()"))
                    .AddFile(FileSyntax.FileDeclaration("RootFile2.cs", "ITest = new Test()"));
            })
            .Create();
    }

    [Fact]
    public void BrConfiguration_WhenInstantiate_ShouldValidateThePropertiesValues()
    {
        // Arrange && Act
        var config = new BrConfiguration("C://ByReplace", ["**//Controllers/*"], []);

        // Assert
        Assert.Equal("C://ByReplace", config.Path);
        Assert.Empty(config.Rules);
        Assert.Single(config.SkipDirectories);
        Assert.Collection(config.SkipDirectories, entry => Assert.Equal("**//Controllers/*", entry));
    }

    [Fact]
    public void SetOnlyOneRule_WhenSetNewRule_ShouldReplaceOldRulesWithTheNewOne()
    {
        // Arrange
        var replacement = new Replacement(["private readonly Test _test;"], "private readonly Test test;");
        var newRule = new ByReplace.Models.Rule("name", "description", ["**//Controller/*"], [".cs", ".py"], replacement);
        var config = new BrConfiguration("C://ByReplace", ["**//Controllers/*"], []);

        // Act
        config.SetOnlyOneRule(newRule);

        // Assert
        Assert.Single(config.Rules);
    }

    [Fact]
    public void ChangeDefaultPath_WhenChangePathToAPathThatExists_ShouldReplaceTheValueOfPath()
    {
        // Arrange
        var config = new BrConfiguration("./ByReplace", ["**//Controllers/*"], []);

        // Act
        config.ChangeDefaultPath("./");

        // Assert
        Assert.Equal("./", config.Path);
    }

    [Fact]
    public void ChangeDefaultPath_WhenChangePathToAPathThatDoesNotExists_ShouldReceiveAnException()
    {
        // Arrange
        var config = new BrConfiguration("C://ByReplace", ["**//Controllers/*"], []);

        // Act
        Action act = () =>
        {
            config.ChangeDefaultPath("//FakeFolder");
        };

        // Assert
        DirectoryNotFoundException ex = Assert.Throws<DirectoryNotFoundException>(act);
        Assert.Equal($"Path //FakeFolder does not exists.", ex.Message);
    }

    [Fact]
    public void GetConfiguration_WhenGetConfigurationThatExistsOnPath_ShouldReturnBrConfiguration()
    {
        // Arrange && Act
        var config = BrConfiguration.GetConfiguration(_workspaceSyntax.Identifier);

        // Assert
        Assert.NotNull(config);
    }

    [Fact]
    public void GetConfiguration_WhenGetConfigurationThatDoesNotExistsOnPath_ShouldReceiveAnException()
    {
        // Arrange
        var config = new BrConfiguration("C://ByReplace", ["**//Controllers/*"], []);

        // Act
        Action act = () =>
        {
            BrConfiguration.GetConfiguration("//FakeFolder");
        };

        // Assert
        FileNotFoundException ex = Assert.Throws<FileNotFoundException>(act);
        Assert.Equal($"BR Configuration not found on //FakeFolder path.", ex.Message);
    }
}
