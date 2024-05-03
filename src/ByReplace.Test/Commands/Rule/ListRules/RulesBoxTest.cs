using Xunit;
using ByReplace.Commands.Rule.ListRules;
using ByReplace.Builders;
using ByReplace.Models;
using ByReplace.Printers;
using ByReplace.Test.Common.ConfigMock;
using ByReplace.Test.Common.FolderMock;
using Moq;
using System.Collections.Immutable;

namespace ByReplace.Test.Commands.Rule.ListRules;

public class RulesBoxTest
{
    private readonly PathCompilationSyntax _pathCompilationSyntax;
    private readonly BrConfiguration _brConfiguration;
    private readonly Mock<IPrint> _printMock;

    public RulesBoxTest()
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
            .AddMembers(
                FileSyntax.FileDeclaration("RootFile1.cs", "ITest = new Test()"),
                FileSyntax.FileDeclaration("RootFile2.cs", "ITest = new Test()"));

        _pathCompilationSyntax = PathFactory
            .Compile(nameof(RulesBoxTest))
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
    public void RulesBox_WhenInstantiate_ShouldValidateTheBoxConfiguration()
    {
        // Arrange & Act
        var rulesBox = new RulesBox(_brConfiguration.Rules);

        // Assert
        Assert.Equal(100, rulesBox.Width);
        Assert.Equal(_brConfiguration.Rules.Count * 2, rulesBox.Height);
        Assert.Equal("Rules", rulesBox.BoxName);
    }

    [Fact]
    public void RulesBox_WhenInstantiate_ShouldValidateIfTwoObjectWithTheSameParametersAreEquals()
    {
        // Arrange
        var rulesBoxFirst = new RulesBox(_brConfiguration.Rules);
        var rulesBoxSecond = new RulesBox(_brConfiguration.Rules);

        // Act
        var isEquals = rulesBoxFirst.Equals(rulesBoxSecond);
        var isEqualsLikeObject = rulesBoxFirst.Equals((object)rulesBoxSecond);
        var hasTheSameHashcode = rulesBoxFirst.GetHashCode() == rulesBoxSecond.GetHashCode();

        // Assert
        Assert.Equal(rulesBoxSecond, rulesBoxFirst);
        Assert.True(isEquals);
        Assert.True(isEqualsLikeObject);
        Assert.True(hasTheSameHashcode);
    }

    [Fact]
    public void RulesBox_WhenInstantiate_ShouldValidateIfTwoObjectWithTheSameParametersAreNotEquals()
    {
        // Arrange
        var rulesBoxFirst = new RulesBox(_brConfiguration.Rules);
        var rulesBoxSecond = new RulesBox([]);

        // Act
        var isEquals = rulesBoxFirst.Equals(rulesBoxSecond);
        var isEqualsLikeObject = rulesBoxFirst.Equals((object)rulesBoxSecond);
        var hasTheSameHashcode = rulesBoxFirst.GetHashCode() == rulesBoxSecond.GetHashCode();

        // Assert
        Assert.NotEqual(rulesBoxSecond, rulesBoxFirst);
        Assert.False(isEquals);
        Assert.False(isEqualsLikeObject);
        Assert.False(hasTheSameHashcode);
    }
}
