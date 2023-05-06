namespace ByReplace.Commands.Apply.Rules;

internal class ApplyRulesCommand : ICommand
{
    private readonly BrConfiguration configuration;
    private readonly IPrint print;

    public ApplyRulesCommand(BrConfiguration configuration, IPrint print)
    {
        this.configuration = configuration;
        this.print = print;
    }

    public ValueTask ExecuteAsync(CancellationToken cancellationToken = default)
    {
        Analyzer analyser = new Analyzer(configuration, print);
        var three = analyser.LoadThreeFiles();

        AnalyzerRunner analyzerRunner = new AnalyzerRunner(configuration, print);
        var fixers = analyzerRunner.RunAnalysis(three, Analyses.Fix);

        DocumentFix analyzerFix = new DocumentFix(fixers, print);
        return analyzerFix.ApplyAsync(cancellationToken);
    }
}
