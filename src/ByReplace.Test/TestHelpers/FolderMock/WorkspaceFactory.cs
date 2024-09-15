namespace ByReplace.Test.TestHelpers.FolderMock;

internal sealed class WorkspaceFactory
{
    public static WorkspaceSyntax Compile() => new WorkspaceSyntax();
    public static WorkspaceSyntax Compile(string testCase) => new WorkspaceSyntax(testCase);
}
