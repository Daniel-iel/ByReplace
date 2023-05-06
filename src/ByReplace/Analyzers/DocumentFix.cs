namespace ByReplace.Analyzers;

internal class DocumentFix
{
    private readonly AnalyzersAndFixers codeFixes;

    public DocumentFix(AnalyzersAndFixers codeFixes)
    {
        this.codeFixes = codeFixes;
    }

    public event EventHandler ApplyResultHandle;

    public ValueTask ApplyAsync(CancellationToken cancellationToken)
    {
        return FindAndReplace(this.codeFixes, cancellationToken);
    }

    public ValueTask ApplyAsync(string rule, CancellationToken cancellationToken)
    {
        var codeFixersFiltered = this.codeFixes.FindByKey(rule);

        return FindAndReplace(codeFixersFiltered, cancellationToken);
    }

    private async ValueTask FindAndReplace(AnalyzersAndFixers codeFixes, CancellationToken cancellationToken)
    {
        foreach (var codeFixe in this.codeFixes)
        {
            Rule role = codeFixe.Key;
            foreach (var file in codeFixe.Value)
            {
                foreach (var removeTerm in role.Replacement.Old)
                {
                    string fileContents = await File.ReadAllTextAsync(file.FullName, cancellationToken);
                    fileContents = fileContents.Replace(removeTerm, role.Replacement.New);
                    await File.WriteAllTextAsync(file.FullName, fileContents, cancellationToken);
                }
            }
        }
    }
}
