using static ByReplace.Mappers.DirectoryThreeV2;

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

    public ImmutableList<SourceThree> GetSourceThree()
    {
        print.Information("Identifying folder three files.");

        DirectoryThreeV2 directoryThree = new DirectoryThreeV2(print);
        directoryThree.MapThreeSources(brConfiguration.Path);

        return directoryThree.Nodes;
    }
}
