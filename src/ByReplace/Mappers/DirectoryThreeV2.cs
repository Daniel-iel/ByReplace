namespace ByReplace.Mappers;

public class DirectoryThreeV2
{
    private readonly IPrint _printer;

    public DirectoryThreeV2(IPrint print)
    {
        _printer = print;
    }

    public record struct SourceThree(string Path);

    public ImmutableList<SourceThree> Nodes { get; private set; }

    public ImmutableList<SourceThree> MapThreeSources(string path)
    {
        List<SourceThree> nodes = new List<SourceThree>();

        MapThreeSubFolders(path, ref nodes);

        Nodes = nodes
            .OrderBy(c => c.Path)
            .ToImmutableList();

        return Nodes;
    }

    private void MapThreeSubFolders(string dir, ref List<SourceThree> nodes)
    {
        DirectoryInfo directoryInfo = new DirectoryInfo(dir);

        var filesFound = directoryInfo.GetFiles().Length;

        var sourceThrees = directoryInfo
                   .GetFiles()
                   .Select(file => new SourceThree(Path: file.FullName))
                     .ToImmutableList();

        _printer.Information($"Found [Cyan]{filesFound} files on folder [Cyan]{dir}.");

        nodes.AddRange(sourceThrees);

        // SubFolders
        foreach (string subDirectory in Directory.GetDirectories(dir))
        {
            MapThreeSubFolders(subDirectory, ref nodes);
        }
    }
}