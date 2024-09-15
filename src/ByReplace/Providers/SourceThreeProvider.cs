namespace ByReplace.Providers;

internal sealed class SourceThreeProvider
{
    private readonly BrConfiguration brConfiguration;
    private readonly IPrint print;

    public SourceThreeProvider(BrConfiguration brConfiguration, IPrint print)
    {
        this.brConfiguration = brConfiguration;
        this.print = print;
    }

    public ImmutableList<DirectoryNode> Run()
    {
        print.Information("Identifying folder three files.");

        DirectoryThree directoryThree = new DirectoryThree(print);
        directoryThree.MapThreeSources(brConfiguration.Path);

        return directoryThree
            .Nodes
            .OrderBy(c => c.Directory)
            .ToImmutableList();
    }
}
