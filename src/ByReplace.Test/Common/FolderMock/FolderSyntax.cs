namespace ByReplace.Test.Common.FolderMock;

internal sealed class FolderSyntax
{
    public FolderSyntax(string name)
    {
        Name = name;
        Files = new List<FileSyntax>();
    }

    public FolderSyntax(string name, FolderSyntax parent)
    {
        Name = name;
        Parent = parent;
        Files = new List<FileSyntax>();
    }

    public FolderSyntax(string name, FolderSyntax parent, List<FileSyntax> files)
    {
        Name = name;
        Parent = parent;
        Files = files;
    }

    public FolderSyntax Parent { get; private set; }

    public string Name { get; private set; }

    public List<FileSyntax> Files { get; private set; }

    public static FolderSyntax FolderDeclaration(string name)
    {
        return new FolderSyntax(name);
    }

    public static FolderSyntax FolderDeclaration(string name, FolderSyntax parent)
    {
        return new FolderSyntax(name, parent);
    }

    public static FolderSyntax FolderDeclaration(string name, FolderSyntax parent, List<FileSyntax> files)
    {
        return new FolderSyntax(name, parent, files);
    }

    public FolderSyntax AddMenbers(FileSyntax fileSyntax)
    {
        this.Files.Add(fileSyntax);
        return this;
    }

    public FolderSyntax AddParent(FolderSyntax parent)
    {
        this.Parent = parent;
        return this;
    }
}
