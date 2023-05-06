using static ByReplace.Mappers.DirectoryThree;

namespace ByReplace.Analyzers;

internal class AnalyzerRunner
{
    private readonly BrConfiguration brConfiguration;

    internal AnalyzerRunner(BrConfiguration brConfiguration)
    {
        this.brConfiguration = brConfiguration;
    }

    internal AnalyzersAndFixers RunAnalysis(ImmutableList<DirectoryNode> directoryThree, Analyses diagnostic)
    {
        AnalyzersAndFixers analyzersAndFixers = new AnalyzersAndFixers();

        foreach (DirectoryNode dir in directoryThree)
        {
            analyzersAndFixers.TryMatchRole(dir, brConfiguration.Rules);
        }

        return analyzersAndFixers;
    }
}
