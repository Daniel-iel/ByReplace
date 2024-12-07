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



    private async ValueTask FindAndReplaceAsync2(AnalyzerAndFixer filesToFix, CancellationToken cancellationToken)
    {
        foreach (var fileToFix in filesToFix)
        {
            FileInfo file = new FileInfo(fileToFix.Key.Path);
            List<Rule> rules = fileToFix.Value;

            print.Information($"Processing file [Cyan]{file.Name}");

            int counter = 1;

            string fileContent = await File.ReadAllTextAsync(file.FullName, cancellationToken);
            ReadOnlySpan<char> fileContentSpan = fileContent.AsSpan();

            foreach (ref var rule in CollectionsMarshal.AsSpan(rules))
            {
                print.Information($"Applying rule [Cyan]{rule.Name} {counter}/{rules.Count} on file [Cyan]{file.Name}.");

                foreach (string removeTerm in rule.Replacement.Old)
                {
                    ReadOnlySpan<char> removeTermSpan = removeTerm.AsSpan();
                    ReadOnlySpan<char> newTermSpan = rule.Replacement.New.AsSpan();

                    fileContentSpan = ReplaceAllOccurrences(fileContentSpan, removeTermSpan, newTermSpan);
                }

                counter++;
            }

            await File.WriteAllTextAsync(file.FullName, fileContentSpan.ToString(), cancellationToken);
        }
    }

    private static string ReplaceAllOccurrences(ReadOnlySpan<char> content, ReadOnlySpan<char> oldTerm, ReadOnlySpan<char> newTerm)
    {
        // Use a StringBuilder to minimize allocations while replacing.
        var result = new StringBuilder(content.Length);
        int start = 0;

        while (true)
        {
            int index = content[start..].IndexOf(oldTerm);
            if (index == -1)
            {
                result.Append(content[start..]); // Append remaining content.
                break;
            }

            // Append content before the match and the replacement term.
            result.Append(content[start..(start + index)]);
            result.Append(newTerm);

            start += index + oldTerm.Length;
        }

        return result.ToString();
    }
}
