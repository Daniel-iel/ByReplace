using System.Runtime.InteropServices;
namespace ByReplace.Test.Common.FolderMock;

internal sealed class PathCompilationSyntax
{
    public List<FolderSyntax> Folders = new List<FolderSyntax>();

    public string InternalIdentifier { get; private set; }

    public PathCompilationSyntax()
    {
        InternalIdentifier = Guid.NewGuid().ToString();
    }

    public PathCompilationSyntax(string testCase)
    {
        InternalIdentifier = $"{testCase}_{Guid.NewGuid()}";
    }

    public PathCompilationSyntax AddFolders(params FolderSyntax[] foldersSyntax)
    {
        this.Folders.AddRange(foldersSyntax);

        return this;
    }

    public PathCompilationSyntax AddFolder(FolderSyntax folderSyntax)
    {
        this.Folders.Add(folderSyntax);

        return this;
    }

    public PathCompilationSyntax AddFolder(string name)
    {
        this.Folders.Add(new FolderSyntax(name));

        return this;
    }

    public PathCompilationSyntax CreateThreeFolder()
    {
        foreach (ref var folder in CollectionsMarshal.AsSpan(Folders))
        {
            CreateThreeFolder(folder);
        }

        //TODO: Código temporário até o ConfigFactory ser implementado.
        File.Copy("./brconfig.json", $"./{InternalIdentifier}/brconfig.json", true);

        return this;
    }

    public PathCompilationSyntax CreateThreeFolder(FolderSyntax folderSyntax)
    {
        if (folderSyntax is null)
        {
            return this;
        }

        if (folderSyntax.Parent is not null)
        {
            CreateThreeFolder(folderSyntax.Parent);
        }

        var dirPath = folderSyntax.Parent is not null
                ? $"./{InternalIdentifier}/{folderSyntax.Parent.Name}/{folderSyntax.Name}"
                : $"./{InternalIdentifier}/{folderSyntax.Name}";

        if (!Directory.Exists(dirPath))
        {
            Directory.CreateDirectory(dirPath);
        }

        foreach (ref var file in CollectionsMarshal.AsSpan(folderSyntax.Files))
        {
            File.WriteAllText($"{dirPath}/{file.Name}", file.Content);
        }

        return this;
    }
}
