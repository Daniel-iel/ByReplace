using ByReplace.Models;
using System.Collections.Immutable;
using Xunit;

namespace ByReplace.Test.Models;

public class BrConfigurationTest
{
    [Fact]
    public void BrConfiguration_WhenInstantiate_ShouldValidateThePropertiesValues()
    {
        // Arrange && Act
        var config = new BrConfiguration("C://ByReplace", ["**//Controllers/*"], ImmutableList<Rule>.Empty);

        // Assert
        Assert.Equal("C://ByReplace", config.Path);
        Assert.Empty(config.Rules);
        Assert.Single(config.SkipDirectories);
        Assert.Collection(config.SkipDirectories, entry =>
        {
            Assert.Equal("**//Controllers/*", entry);
        });
    }

    [Fact]
    public void SetOnlyOneRule_WhenSetNewRule_ShouldReplaceOldRulesWithTheNewOne()
    {
        // Arrange
        var replacement = new Replacement(["private readonly Test _test;"], "private readonly Test test;");
        var newRule = new ByReplace.Models.Rule("name", "description", ["**//Controller/*"], [".cs", ".py"], replacement);
        var config = new BrConfiguration("C://ByReplace", ["**//Controllers/*"], ImmutableList<Rule>.Empty);

        // Act
        config.SetOnlyOneRule(newRule);

        // Assert
        Assert.Single(config.SkipDirectories);
    }
}
