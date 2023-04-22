using ByReplace.Commands.Apply.Parameters;
using ByReplace.Commands.Handlers;

namespace ByReplace.Commands.Apply.Rule
{
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

            AnalyzerRunner analyzerRunner = new AnalyzerRunner(configuration, three);
            var fixers = analyzerRunner.RunAnalysis(Analyses.Fix);

            DocumentFix analyzerFix = new DocumentFix(fixers);
            return analyzerFix.ApplyAsync(cancellationToken);
        }
    }
}
