using ByReplace.Builders;
using ByReplace.Models;
using ByReplace.Test.Common.ConfigMock;
using System.Runtime.InteropServices;
using System.Text.Json;
namespace ByReplace.Test.Common.FolderMock;

internal sealed class WorkspaceSyntax
{
    private readonly List<FolderSyntaxV2> _folders;
    private ContentSyntax _contextSyntax;

    public string Identifier { get; }

    public BrConfiguration BrConfiguration { get; private set; }

    public WorkspaceSyntax()
    {
        Identifier = Guid.NewGuid().ToString();
        _folders = new List<FolderSyntaxV2>();
        _contextSyntax = new ContentSyntax();
    }

    public WorkspaceSyntax(string testCase)
    {
        Identifier = $"{testCase}_{Guid.NewGuid()}";
        _folders = new List<FolderSyntaxV2>();
        _contextSyntax = new ContentSyntax();
    }

    public WorkspaceSyntax FolderStructure(params FolderSyntaxV2[] foldersSyntax)
    {
        this._folders.AddRange(foldersSyntax);

        return this;
    }

    public WorkspaceSyntax Folder(Action<FolderSyntaxV2> action)
    {
        var rootFolder = new FolderSyntaxV2("RootFolder");
        action(rootFolder);
        this._folders.Add(rootFolder);

        return this;
    }

    public WorkspaceSyntax AddBrConfiguration(ContentSyntax configSyntax)
    {
        _contextSyntax = configSyntax;

        return this;
    }

    public WorkspaceSyntax BRContent(Action<ContentSyntax> action)
    {
        var content = new ContentSyntax();
        action(content);
        _contextSyntax = content;

        return this;
    }

    public WorkspaceSyntax Create()
    {
        foreach (ref var folder in CollectionsMarshal.AsSpan(_folders))
        {
            CreateThreeFolder(folder.Name, folder);
        }

        if (_contextSyntax is not null)
        {
            var brConfig = JsonSerializer.Serialize(_contextSyntax, new JsonSerializerOptions
            {
                WriteIndented = true
            });

            File.WriteAllText($"./{Identifier}/brconfig.json", brConfig);
        }

        BrConfiguration = BrConfigurationBuilder
            .Create()
            .SetPath(Identifier)
            .SetConfigPath(Identifier)
            .Build();

        return this;
    }

    public WorkspaceSyntax CreateThreeFolder(string parentFolder, FolderSyntaxV2 folderSyntax)
    {
        if (folderSyntax is null)
        {
            return this;
        }

        var dirPath = $"./{Identifier}/{parentFolder}";

        if (!Directory.Exists(dirPath))
        {
            Directory.CreateDirectory(dirPath);
        }

        foreach (ref var file in CollectionsMarshal.AsSpan(folderSyntax.Files))
        {
            File.WriteAllText(string.Concat(dirPath, "/", file.Name), file.Content);
        }

        foreach (var subFolder in CollectionsMarshal.AsSpan(folderSyntax.Folders))
        {
            CreateThreeFolder(string.Concat(parentFolder, "/", subFolder.Name), subFolder);
        }
        
        return this;
    }
}
