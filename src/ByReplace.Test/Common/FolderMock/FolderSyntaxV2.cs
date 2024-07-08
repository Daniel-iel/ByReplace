namespace ByReplace.Test.Common.FolderMock;

internal sealed class FolderSyntaxV2
{

    public FolderSyntaxV2(string name)
    {
        Name = name;
        Files = new List<FileSyntaxV2>();
        Folders = new List<FolderSyntaxV2>();
    }

    public List<FolderSyntaxV2> Folders { get; private set; }

    public string Name { get; }

    public List<FileSyntaxV2> Files { get; }

    public FolderSyntaxV2 AddFolder(string name, Action<FolderSyntaxV2> action)
    {
        var folderSyntax = new FolderSyntaxV2(name);
        action(folderSyntax);
        this.Folders.Add(folderSyntax);

        return this;
    }

    public FolderSyntaxV2 AddFiles(params FileSyntaxV2[] filesSyntax)
    {
        this.Files.AddRange(filesSyntax);
        return this;
    }

    public FolderSyntaxV2 AddFile(FileSyntaxV2 fileSyntax)
    {
        this.Files.Add(fileSyntax);
        return this;
    }
}
