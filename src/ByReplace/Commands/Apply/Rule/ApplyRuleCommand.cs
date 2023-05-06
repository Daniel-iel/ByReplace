using ByReplace.Commands.Command;

namespace ByReplace.Commands.Apply.Rule;

internal class ApplyRuleCommand : ICommand
{
    private readonly BrConfiguration configuration;
    private readonly ApplyRuleParameters parameters;

    public ApplyRuleCommand(BrConfiguration configuration, ApplyRuleParameters parameters)
    {
        this.configuration = configuration;
        this.parameters = parameters;
    }

    public ValueTask ExecuteAsync(CancellationToken cancellationToken = default)
    {
        Analyzer analyser = new Analyzer(configuration);
        //analyser.FilesFoundNotify += KConsoleNotify.FilesFound;
        var three = analyser.LoadThreeFiles();

        AnalyzerRunner analyzerRunner = new AnalyzerRunner(configuration);
        var fixers = analyzerRunner.RunAnalysis(three, Analyses.Fix);

        DocumentFix analyzerFix = new DocumentFix(fixers);
        return analyzerFix.ApplyAsync(parameters.Rule, cancellationToken);
    }
}
