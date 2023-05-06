using ByReplace.Printers;

namespace ByReplace.Mappers;

public class DirectoryThree
{
    private readonly string _path;
    private readonly PrintConsole _printer;

    public DirectoryThree(string path)
    {
        _path = path;
        _printer = new PrintConsole();
    }

    public record struct DirectoryNode(
        string Directory,
        string Path,
        string Parent,
        ImmutableList<FileMapper> Files
    );

    public List<DirectoryNode> Nodes { get; private set; }

    public void MapThreeSources()
    {
        var nodes = new List<DirectoryNode>();

        MapThreeSubSources(_path, ref nodes);

        Nodes = nodes;
    }

    private void MapThreeSubSources(string dir, ref List<DirectoryNode> nodes)
    {
        var directoryInfo = new DirectoryInfo(dir);

        var node = new DirectoryNode(
            Directory: directoryInfo.FullName,
            Path: directoryInfo.Name,
            Parent: directoryInfo!.Parent!.Name,
            Files: directoryInfo
                  .GetFiles()
                  .Select(file => new FileMapper(Guid.NewGuid(), file.Name, file.FullName, file.Extension))
                  .ToImmutableList()
           );

        _printer.Information($"Found [Cyan]{node.Files.Count} files on folder [Cyan]{node.Directory}.");

        nodes.Add(node);

        // SubFolders
        foreach (var subDirectory in Directory.GetDirectories(dir))
        {
            MapThreeSubSources(subDirectory, ref nodes);
        }
    }
}