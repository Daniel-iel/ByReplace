using ByReplace.Builders;
using ByReplace.Models;
using ByReplace.Test.Commands.Apply.Rules;
using ByReplace.Test.Common.ConfigMock;
using ByReplace.Test.Common.FolderMock;

namespace ByReplace.Test.ClassFixture;

public class WorkspaceFixture : IDisposable
{
    internal WorkspaceSyntax WorkspaceSyntax { get; set; }
    internal BrConfiguration BrConfiguration { get; set; }
    internal ContentSyntax ConfigContent { get; set; }

    public WorkspaceFixture()
    {
        ConfigContent = BrContentFactory
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
            .AddFiles(
                    FileSyntax.FileDeclaration("RootFile1.cs", "ITest = new Test()"),
                    FileSyntax.FileDeclaration("RootFile2.cs", "ITest = new Test()"));

        WorkspaceSyntax = WorkspaceFactory
            .Compile(nameof(ApplyRulesCommandTest))
            .AddMembers(rootFolder)
            .AddBrConfiguration(ConfigContent)
            .Create();

        BrConfiguration = BrConfigurationBuilder
            .Create()
            .SetPath($"./{WorkspaceSyntax.Identifier}")
            .SetConfigPath($"./{WorkspaceSyntax.Identifier}")
            .Build();
    }

    public void ClearPrevious()
    {
        Dispose();
    }

    public void Dispose()
    {
        Directory.Delete($"./{WorkspaceSyntax.Identifier}", true);
    }
}