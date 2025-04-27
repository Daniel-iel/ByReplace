using ByReplace.Builders;
using ByReplace.Models;
using ByReplace.Test.TestHelpers.ConfigMock;
using System.Runtime.InteropServices;
using System.Text.Json;
namespace ByReplace.Test.TestHelpers.FolderMock;

internal sealed class WorkspaceSyntax
{
    private ContentSyntax _contextSyntax;

    public string Identifier { get; }

    public BrConfiguration BrConfiguration { get; private set; }

    public List<FolderSyntax> Folders { get; private set; }

    public List<FileSyntax> Files =>
      Folders
      .SelectMany(folder => folder.Files)
      .ToList();

    public WorkspaceSyntax()
    {
        Identifier = Guid.NewGuid().ToString();
        Folders = new List<FolderSyntax>();
        _contextSyntax = new ContentSyntax();
    }

    public WorkspaceSyntax(string testCase)
    {
        Identifier = $"{testCase}_{Guid.NewGuid()}";
        Folders = new List<FolderSyntax>();
        _contextSyntax = new ContentSyntax();
    }

    public WorkspaceSyntax FolderStructure(params FolderSyntax[] foldersSyntax)
    {
        Folders.AddRange(foldersSyntax);

        return this;
    }

    public WorkspaceSyntax Folder(Action<FolderSyntax> action)
    {
        var rootFolder = new FolderSyntax("RootFolder");
        action(rootFolder);
        Folders.Add(rootFolder);

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
        foreach (ref var folder in CollectionsMarshal.AsSpan(Folders))
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

    public WorkspaceSyntax CreateThreeFolder(string parentFolder, FolderSyntax folderSyntax)
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

        foreach (ref var subFolder in CollectionsMarshal.AsSpan(folderSyntax.Folders))
        {
            CreateThreeFolder(string.Concat(parentFolder, "/", subFolder.Name), subFolder);
        }

        return this;
    }
}
