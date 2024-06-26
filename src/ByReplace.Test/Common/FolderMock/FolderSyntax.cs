﻿namespace ByReplace.Test.Common.FolderMock;

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

    public string Name { get; }

    public List<FileSyntax> Files { get; }

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

    public FolderSyntax AddMembers(FileSyntax fileSyntax)
    {
        this.Files.Add(fileSyntax);
        return this;
    }

    public FolderSyntax AddMembers(params FileSyntax[] filesSyntax)
    {
        this.Files.AddRange(filesSyntax);
        return this;
    }

    public FolderSyntax AddParent(FolderSyntax parent)
    {
        this.Parent = parent;
        return this;
    }
}
