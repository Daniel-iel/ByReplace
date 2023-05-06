namespace ByReplace.Mappers;

public class DirectoryThree
{
    private readonly string _path;

    public DirectoryThree(string path)
    {
        _path = path;
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

        nodes.Add(node);

        // SubFolders
        foreach (var subDirectory in Directory.GetDirectories(dir))
        {
            MapThreeSubSources(subDirectory, ref nodes);
        }
    }
}