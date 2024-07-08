using ByReplace.Commands.Rule.OpenRule;
using ByReplace.Printers;
using ByReplace.Test.Common.ConfigMock;
using ByReplace.Test.Common.FolderMock;
using Moq;
using Xunit;

namespace ByReplace.Test.Commands.Rule.OpenRule;

public class RuleBoxTest
{
    private readonly WorkspaceSyntax _workspaceSyntax;
    private readonly Mock<IPrint> _printMock;

    public RuleBoxTest()
    {
        _printMock = new Mock<IPrint>();

        _workspaceSyntax = new WorkspaceSyntax(nameof(RuleBoxTest))
           .BRContent(c =>
           {
               c.AddPath("")
                .AddSkip("obj", ".bin")
                .AddRules(
                   ruleOne => ruleOne
                              .WithName("RuleOne")
                              .WithExtensions(".cs", ".txt")
                              .WithSkips("**\\Controllers\\*", "bin\\bin1.txt", "obj\\obj2.txt")
                              .WithReplacement(BrContentFactory.Replacement("OldText", "NewText")),
                   ruleTwo => ruleTwo
                              .WithName("RuleTwo")
                              .WithExtensions(".cs")
                              .WithSkips("**\\Controllers\\*", "bin\\bin1.txt", "obj\\obj2.txt")
                              .WithReplacement(BrContentFactory.Replacement("MyOldText", "MyNewText")));
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
    public void RuleBox_WhenInstantiate_ShouldValidateTheBoxConfiguration()
    {
        // Arrange & Act
        var ruleBox = new RuleBox(_workspaceSyntax.BrConfiguration.Rules[0]);

        // Assert
        Assert.Equal(100, ruleBox.Width);
        Assert.Equal(5 * 2, ruleBox.Height);
        Assert.Equal("Rule", ruleBox.BoxName);
    }

    [Fact]
    public void RuleBox_WhenInstantiate_ShouldValidateIfTwoObjectWithTheSameParametersAreEquals()
    {
        // Arrange
        var ruleBoxFirst = new RuleBox(_workspaceSyntax.BrConfiguration.Rules[0]);
        var ruleBoxSecond = new RuleBox(_workspaceSyntax.BrConfiguration.Rules[0]);

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
        var ruleBoxFirst = new RuleBox(_workspaceSyntax.BrConfiguration.Rules[0]);
        var ruleBoxSecond = new RuleBox(_workspaceSyntax.BrConfiguration.Rules.Last());

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
