using ByReplace.Test.Common.ConfigMock;
using System.Runtime.InteropServices;
using System.Text.Json;
namespace ByReplace.Test.Common.FolderMock;

internal sealed class PathCompilationSyntax
{
    private readonly List<FolderSyntax> _folders;
    private ContentSyntax _brConfiguration;

    public string InternalIdentifier { get; }

    public PathCompilationSyntax()
    {
        InternalIdentifier = Guid.NewGuid().ToString();
        _folders = new List<FolderSyntax>();
        _brConfiguration = new ContentSyntax();
    }

    public PathCompilationSyntax(string testCase)
    {
        InternalIdentifier = $"{testCase}_{Guid.NewGuid()}";
        _folders = new List<FolderSyntax>();
        _brConfiguration = new ContentSyntax();
    }

    public PathCompilationSyntax AddMembers(params FolderSyntax[] foldersSyntax)
    {
        this._folders.AddRange(foldersSyntax);

        return this;
    }

    public PathCompilationSyntax AddMember(FolderSyntax folderSyntax)
    {
        _folders.Add(folderSyntax);

        return this;
    }

    public PathCompilationSyntax AddBrConfiguration(ContentSyntax configSyntax)
    {
        _brConfiguration = configSyntax;

        return this;
    }

    public PathCompilationSyntax AddFolder(string name)
    {
        _folders.Add(new FolderSyntax(name));

        return this;
    }

    public PathCompilationSyntax Create()
    {
        foreach (ref var folder in CollectionsMarshal.AsSpan(_folders))
        {
            CreateThreeFolder(folder);
        }

        if (_brConfiguration is not null)
        {
            var brConfig = JsonSerializer.Serialize(_brConfiguration, new JsonSerializerOptions
            {
                WriteIndented = true
            });

            File.WriteAllText($"./{InternalIdentifier}/brconfig.json", brConfig);
        }

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
