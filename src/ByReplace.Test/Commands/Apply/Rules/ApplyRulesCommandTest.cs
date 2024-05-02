using ByReplace.Builders;
using ByReplace.Commands.Apply.Rules;
using ByReplace.Models;
using ByReplace.Printers;
using ByReplace.Test.Common.ConfigMock;
using ByReplace.Test.Common.FolderMock;
using Moq;
using Xunit;

namespace ByReplace.Test.Commands.Apply.Rules;

public class ApplyRulesCommandTEst
{
    private readonly PathCompilationSyntax _pathCompilationSyntax;
    private readonly BrConfiguration _brConfiguration;
    private readonly Mock<IPrint> _printMock;

    public ApplyRulesCommandTEst()
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
            .Compile(nameof(ApplyRulesCommandTEst))
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
    public void Execute_ApplyRulesToFileThree_DoesNotThrowException()
    {
        // Arrange
        var command = new ApplyRulesCommand(_brConfiguration, _printMock.Object);

        // Act
        var executionResult = Record.Exception(() => command.ExecuteAsync());

        // Assert
        Assert.Null(executionResult);
    }
}
