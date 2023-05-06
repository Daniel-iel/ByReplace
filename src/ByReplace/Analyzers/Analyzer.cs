using static ByReplace.Mappers.DirectoryThree;

namespace ByReplace.Analyzers;

internal class Analyzer
{
    private readonly BrConfiguration brConfiguration;

    public Analyzer(BrConfiguration brConfiguration)
    {
        this.brConfiguration = brConfiguration;
    }

    internal ImmutableList<DirectoryNode> LoadThreeFiles()
    {
        var directoryThree = new DirectoryThree(brConfiguration.Path);
        directoryThree.MapThreeSources();

        return directoryThree.Nodes.ToImmutableList();
    }
}
