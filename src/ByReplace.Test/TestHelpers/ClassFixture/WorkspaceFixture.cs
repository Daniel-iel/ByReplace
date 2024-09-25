using ByReplace.Test.TestHelpers.ConfigMock;
using ByReplace.Test.TestHelpers.FolderMock;

namespace ByReplace.Test.TestHelpers.ClassFixture;

public class WorkspaceFixture<TTestClass> : IDisposable
{
    internal WorkspaceSyntax WorkspaceSyntax { get; set; }

    public WorkspaceFixture()
    {
        Console.WriteLine($"WorkspaceFixture: {typeof(TTestClass).Name}");
        WorkspaceSyntax = new WorkspaceSyntax(Guid.NewGuid().ToString())
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
                    .AddFile(FileSyntax.FileDeclaration("RootFile1.cs", "ITest = new Test2()", folderStructure.Name))
                    .AddFile(FileSyntax.FileDeclaration("RootFile2.cs", "ITest = new Test2()", folderStructure.Name));
            })
            .Create();
    }

    public void ClearPrevious()
    {
        Dispose();
    }

    public void Dispose()
    {
        if (!IsFileInUse())
        {
            Directory.Delete($"./{WorkspaceSyntax.Identifier}", true);
        }
    }

    public bool IsFileInUse()
    {
        try
        {
            using FileStream stream = new FileStream($"./{WorkspaceSyntax.Identifier}", FileMode.Open, FileAccess.ReadWrite, FileShare.None);

            return false;
        }
        catch
        {
            return true;
        }
    }
}