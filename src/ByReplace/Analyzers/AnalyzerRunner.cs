[assembly: InternalsVisibleTo("ByReplace.Test")]

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

    internal AnalyzerAndFixer RunAnalysis(ImmutableList<DirectoryNode> directoryThree, Analyses diagnostic)
    {
        print.Information("Identifying rules that has matches.");

        AnalyzerAndFixer analyzersAndFixers = new AnalyzerAndFixer(this.print);

        foreach (DirectoryNode dir in directoryThree)
        {
            analyzersAndFixers.TryMatchRule(dir, brConfiguration.Rules);
        }

        return analyzersAndFixers;
    }
}
