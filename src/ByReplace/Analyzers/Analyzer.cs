[assembly: InternalsVisibleTo("ByReplace.Test")]

namespace ByReplace.Analyzers;

internal class Analyzer
{
    private readonly BrConfiguration brConfiguration;
    private readonly IPrint print;

    public Analyzer(BrConfiguration brConfiguration, IPrint print)
    {
        this.brConfiguration = brConfiguration;
        this.print = print;
    }

    internal ImmutableList<DirectoryNode> LoadThreeFiles()
    {
        print.Information("Identifying folder three files.");

        DirectoryThree directoryThree = new DirectoryThree(brConfiguration.Path);
        directoryThree.MapThreeSources();

        return directoryThree.Nodes.ToImmutableList();
    }
}
