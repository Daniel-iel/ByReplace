namespace ByReplace.Providers;

internal sealed class DocumentFixProvider
{
    private readonly IPrint print;
    private readonly MatchProvider matchProvider;

    public DocumentFixProvider(
        IPrint print,
        MatchProvider matchProvider)
    {
        this.print = print;
        this.matchProvider = matchProvider;
    }

    public ValueTask RunAsync(CancellationToken cancellationToken)
    {
        print.Information("Initializing fixing.");

        var codeFixes = matchProvider.Run();

        return FindAndReplaceAsync(codeFixes, cancellationToken);
    }

    public ValueTask RunAsync(string rule, CancellationToken cancellationToken)
    {
        print.Information("Initializing fixing.");

        var codeFixes = matchProvider.Run();

        AnalyzerAndFixer codeFixersFiltered = codeFixes.FindByRule(rule);

        return FindAndReplaceAsync(codeFixersFiltered, cancellationToken);
    }

    private async ValueTask FindAndReplaceAsync(AnalyzerAndFixer codeFixes, CancellationToken cancellationToken)
    {
        foreach (KeyValuePair<FileMapper, List<Rule>> codeFixe in codeFixes)
        {
            FileMapper file = codeFixe.Key;
            List<Rule> rules = codeFixe.Value;

            print.Information($"Processing file [Cyan]{file.Name}");

            int counter = 1;

            foreach (var rule in rules)
            {
                print.Information($"Applying rule [Cyan]{rule.Name} {counter}/{rules.Count} on file [Cyan]{file.Name}.");

                string fileContents = await File.ReadAllTextAsync(file.FullName, cancellationToken);

                foreach (string removeTerm in rule.Replacement.Old)
                {
                    fileContents = fileContents.Replace(removeTerm, rule.Replacement.New);
                }

                await File.WriteAllTextAsync(file.FullName, fileContents, cancellationToken);

                counter++;
            }
        }
    }
}
