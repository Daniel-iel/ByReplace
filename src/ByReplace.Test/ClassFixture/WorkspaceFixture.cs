using ByReplace.Test.Common.ConfigMock;
using ByReplace.Test.Common.FolderMock;

namespace ByReplace.Test.ClassFixture;

public class WorkspaceFixture<TTestClass> : IDisposable
{
    internal WorkspaceSyntax WorkspaceSyntax { get; set; }

    public WorkspaceFixture()
    {
        WorkspaceSyntax = new WorkspaceSyntax(typeof(TTestClass).Name)
            .BRContent(c =>
            {
                c.AddPath("")
                .AddSkip("obj", ".bin")
                .AddRules(ruleOnde =>
                    ruleOnde
                        .WithName("RuleTest")
                        .WithExtensions(".cs", ".txt")
                        .WithSkips("**\\Controllers\\*", "bin\\bin1.txt", "obj\\obj2.txt")
                        .WithReplacement(BrContentFactory.Replacement("Test", "Test2")));
            })
            .Folder(folderStructure =>
            {
                folderStructure
                    .AddFile(FileSyntaxV2.FileDeclaration("RootFile1.cs", "ITest = new Test()"))
                    .AddFile(FileSyntaxV2.FileDeclaration("RootFile2.cs", "ITest = new Test()"));
            })
            .Create();
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