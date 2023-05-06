namespace ByReplace.Analyzers;

internal class DocumentFix
{
    private readonly AnalyzersAndFixers codeFixes;
    private readonly IPrint print;

    public DocumentFix(AnalyzersAndFixers codeFixes, IPrint print)
    {
        this.codeFixes = codeFixes;
        this.print = print;
    }

    public ValueTask ApplyAsync(CancellationToken cancellationToken)
    {
        print.Information($"Initializing fixing.");

        return FindAndReplace(this.codeFixes, cancellationToken);
    }

    public ValueTask ApplyAsync(string rule, CancellationToken cancellationToken)
    {
        print.Information($"Initializing fixing.");

        var codeFixersFiltered = this.codeFixes.FindByKey(rule);

        return FindAndReplace(codeFixersFiltered, cancellationToken);
    }

    private async ValueTask FindAndReplace(AnalyzersAndFixers codeFixes, CancellationToken cancellationToken)
    {
        foreach (var codeFixe in codeFixes)
        {
            print.Information($"Processing Rule [Cyan]{codeFixe.Key.Name}");

            var pb = new ProgressBar(PbStyle.DoubleLine, codeFixe.Value.Count);

            int counter = 1;

            Rule role = codeFixe.Key;
            foreach (var file in codeFixe.Value)
            {
                pb.Refresh(counter, file.Name);

                foreach (var removeTerm in role.Replacement.Old)
                {
                    string fileContents = await File.ReadAllTextAsync(file.FullName, cancellationToken);
                    fileContents = fileContents.Replace(removeTerm, role.Replacement.New);
                    await File.WriteAllTextAsync(file.FullName, fileContents, cancellationToken);
                }

                counter++;
            }
        }
    }
}
