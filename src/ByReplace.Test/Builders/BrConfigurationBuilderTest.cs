using ByReplace.Builders;
using ByReplace.Test.Common.ConfigMock;
using ByReplace.Test.Common.FolderMock;
using Xunit;

namespace ByReplace.Test.Builders;

public class BrConfigurationBuilderTest
{
    private readonly PathCompilationSyntax _pathCompilationSyntax;

    public BrConfigurationBuilderTest()
    {
        var rootFolder = FolderSyntax.FolderDeclaration("RootFolder");

        var configContent = BrContentFactory
         .CreateDefault()
         .AddConfig(BrContentFactory.ConfigNoPathDeclaration("obj", ".bin"))
         .AddRules(BrContentFactory
                  .Rule("RuleTest")
                  .WithExtensions(".cs")
                  .WithSkips("**\\Controllers\\*")
                  .WithReplacement(BrContentFactory.Replacement("Test", "Test2")))
         .Compile();

        _pathCompilationSyntax = PathFactory
          .Compile(nameof(BrConfigurationBuilderTest))
          .AddMember(rootFolder)
          .AddBrConfiguration(configContent)
          .Create();
    }

    [Fact]
    public void Build_ReturnsCorrectConfiguration()
    {
        // Arrange
        var configFile = $"./{_pathCompilationSyntax.InternalIdentifier}";
        var path = $"./{_pathCompilationSyntax.InternalIdentifier}";
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
        entry =>
        {
            Assert.Equal("obj", entry);
        },
        entry =>
        {
            Assert.Equal(".bin", entry);
        });
        Assert.Collection(brConfiguration.Rules,
        entry =>
        {
            Assert.Equal("RuleTest", entry.Name);
            Assert.Null(entry.Description);
            Assert.Collection(entry.Skip, entrySkip =>
            {
                Assert.Equal("**\\Controllers\\*", entrySkip);
            });
            Assert.Collection(entry.Extensions, entrySkip =>
            {
                Assert.Equal(".cs", entrySkip);
            });
            Assert.Equal("Test", entry.Replacement.New);
            Assert.Single(entry.Replacement.Old);
            Assert.Collection(entry.Replacement.Old, entry =>
            {
                Assert.Equal("Test2", entry);
            });
        });
    }
}
