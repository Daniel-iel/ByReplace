using ByReplace.Builders;
using ByReplace.Models;
using ByReplace.Test.Analyzers;
using ByReplace.Test.Common.ConfigMock;
using ByReplace.Test.Common.FolderMock;
using System.Collections.Immutable;
using Xunit;

namespace ByReplace.Test.Models;

public class BrConfigurationTest
{
    private readonly PathCompilationSyntax _pathCompilationSyntax;
    private readonly BrConfiguration _brConfiguration;

    public BrConfigurationTest()
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
            .Compile(nameof(BrConfigurationTest))
            .AddMembers(rootFolder)
            .AddBrConfiguration(configContent)
            .Create();

        _brConfiguration = BrConfigurationBuilder
            .Create()
            .SetPath($"./{_pathCompilationSyntax.InternalIdentifier}")
            .SetConfigPath($"./{_pathCompilationSyntax.InternalIdentifier}")
            .Build();
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
        Assert.Single(config.SkipDirectories);
    }

    [Fact]
    public void ChangeDefaultPath_WhenChangePathToAPathThatExists_ShouldReplaceTheValueOfPath()
    {
        // Arrange
        var config = new BrConfiguration("C://ByReplace", ["**//Controllers/*"], []);

        // Act
        config.ChangeDefaultPath("C:");

        // Assert
        Assert.Equal("C:", config.Path);
    }

    [Fact]
    public void ChangeDefaultPath_WhenChangePathToAPathThatDoesNotExists_ShouldReceiveAnException()
    {
        // Arrange
        var config = new BrConfiguration("C://ByReplace", ["**//Controllers/*"], []);

        // Act && Assert
        Assert.Throws<DirectoryNotFoundException>(() => config.ChangeDefaultPath("//FakeFolder"));
    }

    [Fact]
    public void GetConfiguration_WhenGetConfigurationThatExistsOnPath_ShouldReturnBrConfiguration()
    {
        // Arrange && Act
        var config = BrConfiguration.GetConfiguration(_pathCompilationSyntax.InternalIdentifier);

        // Assert
        Assert.NotNull(config);
    }

    [Fact]
    public void GetConfiguration_WhenGetConfigurationThatDoesNotExistsOnPath_ShouldReceiveAnException()
    {
        // Arrange
        var config = new BrConfiguration("C://ByReplace", ["**//Controllers/*"], []);

        // Act && Assert
        Assert.Throws<FileNotFoundException>(() => BrConfiguration.GetConfiguration("//FakeFolder"));
    }
}
