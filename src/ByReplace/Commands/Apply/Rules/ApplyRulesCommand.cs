using ByReplace.Commands.Command;

namespace ByReplace.Commands.Apply.Rules;

internal class ApplyRulesCommand : ICommand
{
    private readonly BrConfiguration configuration;
    private readonly ApplyParameters applyParameters;

    public ApplyRulesCommand(BrConfiguration configuration, ApplyParameters applyParameters)
    {
        this.configuration = configuration;
        this.applyParameters = applyParameters;
    }

    public ValueTask ExecuteAsync(CancellationToken cancellationToken = default)
    {
        Analyzer analyser = new Analyzer(configuration);
        var three = analyser.LoadThreeFiles();

        AnalyzerRunner analyzerRunner = new AnalyzerRunner(configuration);
        var fixers = analyzerRunner.RunAnalysis(three, Analyses.Fix);

        DocumentFix analyzerFix = new DocumentFix(fixers);
        return analyzerFix.ApplyAsync(cancellationToken);
    }
}
