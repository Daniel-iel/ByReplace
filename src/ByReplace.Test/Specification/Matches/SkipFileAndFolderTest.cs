using ByReplace.Specification.Matches;
using ByReplace.Test.TestHelpers.Builders;
using Xunit;

namespace ByReplace.Test.Specification.Matches;
public class SkipFileAndFolderTest
{
    [Theory]
    [InlineData("C:\\dir\\test.txt", new string[] { "test.txt" })]
    [InlineData("C:\\dir\\test.txt", new string[] { "\\dir\\test.txt" })]
    [InlineData("C:\\dir\\test.txt", new string[] { "\\dir\\test.txt", "\\dir\\test5.txt" })]
    public void IsSatisfiedBy_WhenMatchTheFolderWithFile_ShouldReturnTrue(string fullName, string[] skip)
    {
        // Arrange
        var rule = RuleBuilderTest
            .Create()
            .WithName("Remove")
            .WithDescription("ToRemove")
            .WithSkip(skip)
            .WithExtensions(".txt")
            .WithReplacement(c =>
            {
                c = c with
                {
                    Old = ["_test.", "this._test"],
                    New = "_test."
                };
            })
        .Build();

        var spec = new FileWithFolderSpec(fullName);

        // Act
        var isSatisfied = spec.IsSatisfiedBy(rule);

        // Assert
        Assert.True(isSatisfied);
    }

    [Theory]
    [InlineData("/dir/test.txt", new string[] { "test1.txt", "test2.txt" })]
    [InlineData("/dir/test.txt", new string[] { "**\\dir\\*", "test1.txt" })]
    [InlineData("/dir/test.txt", new string[] { "**//dir//*", "test1.txt" })]
    [InlineData("/dir/test.txt", new string[] { "*\\dir\\*", "test1.txt" })]
    [InlineData("/dir/test.txt", new string[] { "*//dir//*", "test1.txt" })]
    [InlineData("/dir/test.txt", new string[] { "*\\dir\\", "test1.txt" })]
    [InlineData("/dir/test.txt", new string[] { "*//dir//", "test1.txt" })]
    [InlineData("/dir/test.txt", new string[] { "\\dir\\", "test1.txt" })]
    [InlineData("/dir/test.txt", new string[] { "//dir//", "test1.txt" })]
    [InlineData("/dir/test1.txt", new string[] { "/folder/test.txt", "test5.txt" })]
    public void IsSatisfiedBy_WhenDoesNotMatchTheFolderWithFile_ShouldReturnFalse(string fullName, string[] skip)
    {
        // Arrange
        var rule = RuleBuilderTest
            .Create()
            .WithName("Remove")
            .WithDescription("ToRemove")
            .WithSkip(skip)
            .WithExtensions(".txt")
            .WithReplacement(c =>
            {
                c = c with
                {
                    Old = ["_test.", "this._test"],
                    New = "_test."
                };
            })
            .Build();

        var spec = new FileWithFolderSpec(fullName);

        // Act
        var result = spec.IsSatisfiedBy(rule);

        // Assert
        Assert.False(result);
    }
}