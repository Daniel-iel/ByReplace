namespace ByReplace.Mappers;

public class DirectoryThree
{
    private readonly IPrint _printer;

    public DirectoryThree(IPrint print)
    {
        _printer = print;
    }

    public record struct DirectoryNode(
        string Directory,
        string Path,
        string Parent,
        ImmutableList<FileMapper> Files
    );

    public ImmutableList<DirectoryNode> Nodes { get; private set; }

    public ImmutableList<DirectoryNode> MapThreeSources(string path)
    {
        List<DirectoryNode> nodes = new List<DirectoryNode>();

        MapThreeSubFolders(path, ref nodes);

        Nodes = nodes.ToImmutableList();

        return Nodes;
    }

    private void MapThreeSubFolders(string dir, ref List<DirectoryNode> nodes)
    {
        DirectoryInfo directoryInfo = new DirectoryInfo(dir);

        DirectoryNode node = new DirectoryNode(
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
        foreach (string subDirectory in Directory.GetDirectories(dir))
        {
            MapThreeSubFolders(subDirectory, ref nodes);
        }
    }
}