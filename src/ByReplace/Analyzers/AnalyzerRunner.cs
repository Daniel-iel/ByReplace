namespace ByReplace.Analyzers;

internal class AnalyzerRunner
{
    private readonly BrConfiguration brConfiguration;
    private readonly IPrint print;

    internal AnalyzerRunner(BrConfiguration brConfiguration, IPrint print)
    {
        this.brConfiguration = brConfiguration;
        this.print = print;
    }

    internal AnalyzersAndFixers RunAnalysis(ImmutableList<DirectoryNode> directoryThree, Analyses diagnostic)
    {
        print.Information($"Identifying folder three files.");

        AnalyzersAndFixers analyzersAndFixers = new AnalyzersAndFixers(this.print);

        foreach (DirectoryNode dir in directoryThree)
        {
            analyzersAndFixers.TryMatchRole(dir, brConfiguration.Rules);
        }

        return analyzersAndFixers;
    }
}
