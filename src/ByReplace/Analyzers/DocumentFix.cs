[assembly: InternalsVisibleTo("ByReplace.Test")]

namespace ByReplace.Analyzers;

internal class DocumentFix
{
    private readonly AnalyzerAndFixer codeFixes;
    private readonly IPrint print;

    public DocumentFix(AnalyzerAndFixer codeFixes, IPrint print)
    {
        this.codeFixes = codeFixes;
        this.print = print;
    }

    public ValueTask ApplyAsync(CancellationToken cancellationToken)
    {
        print.Information($"Initializing fixing.");

        return FindAndReplaceAsync(this.codeFixes, cancellationToken);
    }

    public ValueTask ApplyAsync(string rule, CancellationToken cancellationToken)
    {
        print.Information($"Initializing fixing.");

        AnalyzerAndFixer codeFixersFiltered = this.codeFixes.FindByKey(rule);

        return FindAndReplaceAsync(codeFixersFiltered, cancellationToken);
    }

    private async ValueTask FindAndReplaceAsync(AnalyzerAndFixer codeFixes, CancellationToken cancellationToken)
    {
        foreach (KeyValuePair<FileMapper, List<Rule>> codeFixe in codeFixes)
        {
            FileMapper file = codeFixe.Key;
            IReadOnlyList<Rule> rules = codeFixe.Value;

            print.Information($"Processing file [Cyan]{file.Name}");

            int counter = 1;

            foreach (var rule in rules)
            {
                print.Information($"Appling rule [Cyan]{rule.Name} {counter}/{rules.Count} on file [Cyan]{file.Name}.");

                foreach (string removeTerm in rule.Replacement.Old)
                {
                    string fileContents = await File.ReadAllTextAsync(file.FullName, cancellationToken);
                    fileContents = fileContents.Replace(removeTerm, rule.Replacement.New);
                    await File.WriteAllTextAsync(file.FullName, fileContents, cancellationToken);
                }

                counter++;
            }
        }
    }
}
