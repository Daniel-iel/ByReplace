namespace ByReplace.Test.Common.FolderMock;

internal sealed class PathFactory
{
    public static PathCompilationSyntax Compile() => new PathCompilationSyntax();
    public static PathCompilationSyntax Compile(string testCase) => new PathCompilationSyntax(testCase);
}
