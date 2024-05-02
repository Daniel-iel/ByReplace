using ByReplace.Builders;
using ByReplace.Commands.Rule.ListRules;
using ByReplace.Commands.Rule.OpenRule;
using ByReplace.Models;
using ByReplace.Printers;
using ByReplace.Test.Analyzers;
using ByReplace.Test.Common.ConfigMock;
using ByReplace.Test.Common.FolderMock;
using Moq;
using System.Collections.Immutable;
using Xunit;

namespace ByReplace.Test.Commands.Rule.OpenRule;

public class RuleBoxTest
{
    private readonly PathCompilationSyntax _pathCompilationSyntax;
    private readonly BrConfiguration _brConfiguration;
    private readonly Mock<IPrint> _printMock;

    public RuleBoxTest()
    {
        _printMock = new Mock<IPrint>();

        var configContent = BrContentFactory
          .CreateDefault()
          .AddConfig(BrContentFactory.ConfigNoPathDeclaration("obj", ".bin"))
          .AddRules(BrContentFactory
                .Rule("RuleOne")
                .WithExtensions(".cs", ".txt")
                .WithSkips("**\\Controllers\\*", "bin\\bin1.txt", "obj\\obj2.txt")
                .WithReplacement(BrContentFactory.Replacement("OldText", "NewText")),
                BrContentFactory
                .Rule("RuleTwo")
                .WithExtensions(".cs")
                .WithSkips("**\\Controllers\\*", "bin\\bin1.txt", "obj\\obj2.txt")
                .WithReplacement(BrContentFactory.Replacement("MyOldText", "MyNewText")))
          .Compile();

        var rootFolder = FolderSyntax
            .FolderDeclaration("RootFolder")
            .AddMembers(
                FileSyntax.FileDeclaration("RootFile1.cs", "ITest = new Test()"),
                FileSyntax.FileDeclaration("RootFile2.cs", "ITest = new Test()"));

        _pathCompilationSyntax = PathFactory
            .Compile(nameof(AnalyzerAndFixerTest))
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
    public void RuleBox_WhenInstantiate_ShouldValidateTheBoxConfiguration()
    {
        // Arrange & Act
        var ruleBox = new RuleBox(_brConfiguration.Rules.First());

        // Assert
        Assert.Equal(100, ruleBox.Width);
        Assert.Equal(5 * 2, ruleBox.Height);
        Assert.Equal("Rule", ruleBox.BoxName);
    }

    [Fact]
    public void RuleBox_WhenInstantiate_ShouldValidateIfTwoObjectWithTheSameParametersAreEquals()
    {
        // Arrange
        var ruleBoxFirst = new RuleBox(_brConfiguration.Rules.First());
        var ruleBoxSecond = new RuleBox(_brConfiguration.Rules.First());

        // Act
        var isEquals = ruleBoxFirst.Equals(ruleBoxSecond);
        var isEqualsLikeObject = ruleBoxFirst.Equals((object)ruleBoxSecond);
        var hasTheSameHashcode = ruleBoxFirst.GetHashCode() == ruleBoxSecond.GetHashCode();

        // Assert
        Assert.Equal(ruleBoxSecond, ruleBoxFirst);
        Assert.True(isEquals);
        Assert.True(isEqualsLikeObject);
        Assert.True(hasTheSameHashcode);
    }

    [Fact]
    public void RuleBox_WhenInstantiate_ShouldValidateIfTwoObjectWithTheSameParametersAreNotEquals()
    {
        // Arrange
        var ruleBoxFirst = new RuleBox(_brConfiguration.Rules.First());
        var ruleBoxSecond = new RuleBox(_brConfiguration.Rules.Last());

        // Act
        var isEquals = ruleBoxFirst.Equals(ruleBoxSecond);
        var isEqualsLikeObject = ruleBoxFirst.Equals((object)ruleBoxSecond);
        var hasTheSameHashcode = ruleBoxFirst.GetHashCode() == ruleBoxSecond.GetHashCode();

        // Assert
        Assert.NotEqual(ruleBoxSecond, ruleBoxFirst);
        Assert.False(isEquals);
        Assert.False(isEqualsLikeObject);
        Assert.False(hasTheSameHashcode);
    }
}
