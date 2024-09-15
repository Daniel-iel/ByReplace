namespace ByReplace.Providers;

internal sealed class MatchProvider
{
    private readonly BrConfiguration brConfiguration;
    private readonly IPrint print;
    private readonly SourceThreeProvider sourceThreeProvider;

    internal MatchProvider(BrConfiguration brConfiguration, IPrint print, SourceThreeProvider sourceThreeProvider)
    {
        this.brConfiguration = brConfiguration;
        this.print = print;
        this.sourceThreeProvider = sourceThreeProvider;
    }

    public AnalyzerAndFixer Run()
    {
        print.Information("Identifying rules that has matches.");

        AnalyzerAndFixer analyzersAndFixers = new AnalyzerAndFixer(print, brConfiguration.Rules);

        foreach (var sourceFile in sourceThreeProvider.Run())
        {
            analyzersAndFixers.TryMatchRule(sourceFile);
        }

        return analyzersAndFixers;
    }
}
