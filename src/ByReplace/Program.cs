using ByReplace.Builders;
using Cocona;

var app = CoconaApp.Create(new[] { "apply", "rule", "-r RemoveServiceBus", @"-p C:\Users\iel_1\Documents\TestLieu", @"-f C:\Users\iel_1\Documents\Projetos\ByReplace\ByReplace" });

const string RULE_DESCRIPTION = "Rule name to be applied.";
const string PATH_DESCRIPTION = "Path of the files to be applied to the rule.";
const string FILE_DESCRIPTION = "Path of the brconfig file.";

app
    .AddSubCommand("apply", apply =>
    {
        apply.AddCommand("rule", (
            [Option(shortName: 'r', Description = RULE_DESCRIPTION)] string rule,
            [Option(shortName: 'p', Description = PATH_DESCRIPTION)] string path,
            [Option(shortName: 'f', Description = FILE_DESCRIPTION)] string configFile) =>
        {
            var configuration = BrConfigurationBuilder
                .Instantiate()
                .SetRule(rule)
                .SetPath(path)
                .SetConfigPath(configFile)
                .Build();

            Analyzer analyser = new Analyzer(configuration);
            var three = analyser.LoadThreeFiles();

            AnalyzerRunner analyzerRunner = new AnalyzerRunner(configuration, three);
            var fixers = analyzerRunner.RunAnalysis(Analyses.Fix);

            DocumentFix analyzerFix = new DocumentFix(fixers);
            analyzerFix.Apply();
        });

        apply.AddCommand("rules", (
            [Option(shortName: 'p', Description = PATH_DESCRIPTION)] string path,
            [Option(shortName: 'f', Description = FILE_DESCRIPTION)] string configFile) =>
        {
            var configuration = BrConfigurationBuilder
               .Instantiate()
               .SetPath(path)
               .SetConfigPath(configFile)
               .Build();

            Analyzer analyser = new Analyzer(configuration);
            var three = analyser.LoadThreeFiles();

            AnalyzerRunner analyzerRunner = new AnalyzerRunner(configuration, three);
            var fixers = analyzerRunner.RunAnalysis(Analyses.Fix);

            DocumentFix analyzerFix = new DocumentFix(fixers);
            analyzerFix.Apply();
        });
    })
    .WithDescription("apply commands");

app.AddCommand("verify", () => { throw new NotImplementedException("verify command not implemented."); });
app.AddCommand("take", () => { throw new NotImplementedException("take command not implemented."); });

@app.Run();