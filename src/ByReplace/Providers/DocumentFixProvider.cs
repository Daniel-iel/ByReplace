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
        foreach (KeyValuePair<DirectoryThreeV2.SourceThree, List<Rule>> codeFixe in codeFixes)
        {
            FileInfo file = new FileInfo(codeFixe.Key.Path);
            List<Rule> rules = codeFixe.Value;

            print.Information($"Processing file [Cyan]{file.Name}");

            int counter = 1;

            string fileContents = await File.ReadAllTextAsync(file.FullName, cancellationToken);

            foreach (var rule in rules)
            {
                print.Information($"Applying rule [Cyan]{rule.Name} {counter}/{rules.Count} on file [Cyan]{file.Name}.");

                foreach (string removeTerm in rule.Replacement.Old)
                {
                    fileContents = fileContents.Replace(removeTerm, rule.Replacement.New);
                }

                counter++;
            }

            await File.WriteAllTextAsync(file.FullName, fileContents, cancellationToken);
        }
    }

    //private async ValueTask FindAndReplaceAsync(AnalyzerAndFixer codeFixes, CancellationToken cancellationToken)
    //{
    //    foreach (KeyValuePair<FileMapper, List<Rule>> codeFixe in codeFixes)
    //    {
    //        FileMapper file = codeFixe.Key;
    //        List<Rule> rules = codeFixe.Value;

    //        print.Information($"Processing file [Cyan]{file.Name}");

    //        int counter = 1;

    //        await using (FileStream readStream = new FileStream(file.FullName, FileMode.Open, FileAccess.ReadWrite, FileShare.None))
    //        using (StreamReader reader = new StreamReader(readStream))
    //        {
    //            string fileContents = string.Empty;

    //            foreach (var rule in rules)
    //            {
    //                print.Information($"Applying rule [Cyan]{rule.Name} {counter}/{rules.Count} on file [Cyan]{file.Name}.");

    //                fileContents = await reader.ReadToEndAsync();

    //                foreach (string removeTerm in rule.Replacement.Old)
    //                {
    //                    fileContents = fileContents.Replace(removeTerm, rule.Replacement.New);
    //                }
    //            }

    //            await using FileStream writeStream = new FileStream(file.FullName, FileMode.Create, FileAccess.ReadWrite, FileShare.None);
    //            await using StreamWriter writer = new StreamWriter(writeStream);
    //            await writer.WriteAsync(fileContents);

    //            counter++;
    //        }
    //    }
    //}
}
