using System.Runtime.InteropServices;
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

    [MethodImpl(MethodImplOptions.AggressiveOptimization)]
    private async ValueTask FindAndReplaceAsync(AnalyzerAndFixer filesToFix, CancellationToken cancellationToken)
    {
        foreach (var fileToFix in filesToFix)
        {
            FileInfo file = new FileInfo(fileToFix.Key.Path);
            List<Rule> rules = fileToFix.Value;

            print.Information($"Processing file [Cyan]{file.Name}");

            int counter = 1;

            string fileContent = await File.ReadAllTextAsync(file.FullName, cancellationToken);

            foreach (ref var rule in CollectionsMarshal.AsSpan(rules))
            {
                print.Information($"Applying rule [Cyan]{rule.Name} {counter}/{rules.Count} on file [Cyan]{file.Name}.");

                foreach (string removeTerm in rule.Replacement.Old)
                {
                    fileContent = fileContent.Replace(removeTerm, rule.Replacement.New);
                }

                counter++;
            }

            await File.WriteAllTextAsync(file.FullName, fileContent, cancellationToken);
        }
    }
}
