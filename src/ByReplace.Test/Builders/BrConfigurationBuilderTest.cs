using ByReplace.Builders;
using ByReplace.Test.TestHelpers.ConfigMock;
using ByReplace.Test.TestHelpers.FolderMock;
using Xunit;

namespace ByReplace.Test.Builders;

public class BrConfigurationBuilderTest
{
    private readonly WorkspaceSyntax _workspaceSyntax;

    public BrConfigurationBuilderTest()
    {
        _workspaceSyntax = new WorkspaceSyntax(nameof(BrConfigurationBuilderTest))
            .BRContent(c =>
            {
                c.AddPath("")
                 .AddSkip("obj", ".bin")
                 .AddRules(
                    ruleOne => ruleOne
                               .WithName("RuleTest")
                               .WithExtensions(".cs")
                               .WithSkips("**\\Controllers\\*")
                               .WithReplacement(BrContentFactory.Replacement("Test", "Test2")));
            })
            .Folder(folderStructure =>
            {
            })
            .Create();
    }

    [Fact]
    public void Build_WhenPassCorrectInformation_ShouldReturnTheBrConfiguration()
    {
        // Arrange
        var configFile = $"./{_workspaceSyntax.Identifier}";
        var path = $"./{_workspaceSyntax.Identifier}";
        const string rule = "RuleTest";

        var builder = BrConfigurationBuilder
            .Create()
            .SetConfigPath(configFile + " ")
            .SetPath(path + " ")
            .SetRule(rule + " ");

        // Act
        var brConfiguration = builder.Build();

        // Assert
        Assert.Equal(configFile, brConfiguration.Path);
        Assert.Collection(brConfiguration.SkipDirectories,
        entry => Assert.Equal("obj", entry),
        entry => Assert.Equal(".bin", entry));
        Assert.Collection(brConfiguration.Rules,
        entry =>
        {
            Assert.Equal("RuleTest", entry.Name);
            Assert.Null(entry.Description);
            Assert.Collection(entry.Skip, entrySkip => Assert.Equal("**\\Controllers\\*", entrySkip));
            Assert.Collection(entry.Extensions, entrySkip => Assert.Equal(".cs", entrySkip));
            Assert.Equal("Test", entry.Replacement.New);
            Assert.Single(entry.Replacement.Old);
            Assert.Collection(entry.Replacement.Old, entry => Assert.Equal("Test2", entry));
        });
    }

    [Theory]
    [InlineData("", "./", "Rule")]
    [InlineData("./", "", "Rule")]
    [InlineData("./", "./", "")]
    public void Build_WhenPassTheIncorrectInformations_ShouldThrowArgumentException(string pathConfig, string path, string rule)
    {
        Action action = () =>
        {
            // Arrange
            var builder = BrConfigurationBuilder
              .Create()
              .SetConfigPath(pathConfig)
              .SetPath(path)
              .SetRule(rule);

            // Act
            builder.Build();
        };

        Assert.Throws<ArgumentException>(action);
    }
}
