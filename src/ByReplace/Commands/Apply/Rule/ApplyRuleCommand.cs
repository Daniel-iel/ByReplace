namespace ByReplace.Commands.Apply.Rule;

internal class ApplyRuleCommand : ICommand
{
    private readonly BrConfiguration configuration;
    private readonly ApplyRuleParameter parameters;
    private readonly IPrint print;

    public ApplyRuleCommand(
        BrConfiguration configuration,
        ApplyRuleParameter parameters,
        IPrint print)
    {
        this.configuration = configuration;
        this.parameters = parameters;
        this.print = print;
    }

    public ValueTask ExecuteAsync(CancellationToken cancellationToken = default)
    {
        Analyzer analyser = new Analyzer(configuration, print);
        ImmutableList<DirectoryNode> three = analyser.LoadThreeFiles();

        AnalyzerRunner analyzerRunner = new AnalyzerRunner(configuration, print);
        AnalyzerAndFixer fixers = analyzerRunner.RunAnalysis(three, Analyses.Fix);

        DocumentFix analyzerFix = new DocumentFix(fixers, print);
        return analyzerFix.ApplyAsync(parameters.Rule, cancellationToken);
    }
}
