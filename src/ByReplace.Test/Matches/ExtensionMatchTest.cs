using ByReplace.Matches;
using Xunit;

namespace ByReplace.Test.Matches;

public class ExtensionMatchTest
{
    [Theory]
    [InlineData(".cs", ".cs", ".text", ".py")]
    [InlineData(".text", ".cs", ".text", ".py")]
    [InlineData(".py", ".cs", ".text", ".py")]
    public void HasMatch_WhenTryMatchTheAllowExtensionFromFile_ShouldMatch(string extension, params string[] extensions)
    {
        // Arrange
        var match = new ExtensionMatch(extension, extensions);

        // Act
        var hasMatch = match.HasMatch;

        // Assert
        Assert.True(hasMatch);
    }

    [Theory]
    [InlineData(".cs", ".text", ".py")]
    [InlineData(".text", ".cs", ".py")]
    [InlineData(".py", ".cs", ".text")]
    public void HasMatch_WhenTryMatchTheDenyExtensionFromFile_ShouldNotMatch(string extension, params string[] extensions)
    {
        // Arrange
        var match = new ExtensionMatch(extension, extensions);

        // Act
        var hasMatch = match.HasMatch;

        // Assert
        Assert.False(hasMatch);
    }
}
