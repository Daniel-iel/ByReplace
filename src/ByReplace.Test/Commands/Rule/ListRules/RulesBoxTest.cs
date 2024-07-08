using ByReplace.Commands.Rule.ListRules;
using ByReplace.Printers;
using ByReplace.Test.Common.ConfigMock;
using ByReplace.Test.Common.FolderMock;
using Moq;
using Xunit;

namespace ByReplace.Test.Commands.Rule.ListRules;

public class RulesBoxTest
{
    private readonly WorkspaceSyntax _workspaceSyntax;
    private readonly Mock<IPrint> _printMock;

    public RulesBoxTest()
    {
        _printMock = new Mock<IPrint>();

        _workspaceSyntax = new WorkspaceSyntax(nameof(RulesBoxTest))
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
    public void RulesBox_WhenInstantiate_ShouldValidateTheBoxConfiguration()
    {
        // Arrange & Act
        var rulesBox = new RulesBox(_workspaceSyntax.BrConfiguration.Rules);

        // Assert
        Assert.Equal(100, rulesBox.Width);
        Assert.Equal(_workspaceSyntax.BrConfiguration.Rules.Count * 2, rulesBox.Height);
        Assert.Equal("Rules", rulesBox.BoxName);
    }

    [Fact]
    public void RulesBox_WhenInstantiate_ShouldValidateIfTwoObjectWithTheSameParametersAreEquals()
    {
        // Arrange
        var rulesBoxFirst = new RulesBox(_workspaceSyntax.BrConfiguration.Rules);
        var rulesBoxSecond = new RulesBox(_workspaceSyntax.BrConfiguration.Rules);

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
        var rulesBoxFirst = new RulesBox(_workspaceSyntax.BrConfiguration.Rules);
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
