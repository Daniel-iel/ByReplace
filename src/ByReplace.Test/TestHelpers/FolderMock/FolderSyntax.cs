namespace ByReplace.Test.TestHelpers.FolderMock;

internal sealed class FolderSyntax
{

    public FolderSyntax(string name)
    {
        Name = name;
        Files = new List<FileSyntax>();
        Folders = new List<FolderSyntax>();
    }

    public List<FolderSyntax> Folders { get; private set; }

    public string Name { get; }

    public List<FileSyntax> Files { get; }

    public FolderSyntax AddFolder(string name, Action<FolderSyntax> action)
    {
        var folderSyntax = new FolderSyntax(name);
        action(folderSyntax);
        Folders.Add(folderSyntax);

        return this;
    }

    public FolderSyntax AddFiles(params FileSyntax[] filesSyntax)
    {
        Files.AddRange(filesSyntax);
        return this;
    }

    public FolderSyntax AddFile(FileSyntax fileSyntax)
    {
        Files.Add(fileSyntax);
        return this;
    }
}
