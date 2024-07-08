using ByReplace.Test.Common.ConfigMock;
using ByReplace.Test.Common.FolderMock;

namespace ByReplace.Test.Common.Helpers;

internal class WorkspaceMock<TTestClass>
{
    private ContentSyntax _contentSyntax;
    private FolderSyntaxV2 _folderSyntaxV2;

    public WorkspaceMock()
    {
    }

    public WorkspaceMock<TTestClass> BRContent(Action<ContentSyntax> action)
    {
        var content = new ContentSyntax();
        action(content);
        _contentSyntax = content;

        return this;
    }

    public WorkspaceMock<TTestClass> Folder(Action<FolderSyntaxV2> action)
    {
        var rootFolder = new FolderSyntaxV2("RootFolder");
        action(rootFolder);
        _folderSyntaxV2 = rootFolder;

        return this;
    }

    public WorkspaceMock<TTestClass> Create(Action<FolderSyntaxV2> action) { return this; }
}
