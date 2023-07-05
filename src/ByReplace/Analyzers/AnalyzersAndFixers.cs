namespace ByReplace.Analyzers;

internal class AnalyzersAndFixers : Dictionary<Rule, List<FileMapper>>
{
    private readonly IPrint print;

    public AnalyzersAndFixers(IPrint print) : this(Enumerable.Empty<KeyValuePair<Rule, List<FileMapper>>>(), print)
    {
    }

    public AnalyzersAndFixers(IEnumerable<KeyValuePair<Rule, List<FileMapper>>> values, IPrint print) : base(values)
    {
        this.print = print;
    }

    public bool TryMatchRole(DirectoryNode directoryNode, ImmutableList<Rule> roles)
    {
        foreach (FileMapper file in directoryNode.Files)
        {
            foreach (Rule role in roles)
            {
                Match skipDirMatch = new SkipMatch(directoryNode.Directory, file, role.Skip);
                Match extensionMatch = new ExtensionMatch(file.Extension, role.Extensions);

                if (skipDirMatch.HasMatch || !extensionMatch.HasMatch)
                {
                    continue;
                }

                if (!this.ContainsKey(role))
                {
                    this.Add(role, new List<FileMapper>());
                }

                this[role].Add(file);
            }
        }

        Print();

        return false;
    }

    private void Print()
    {
        foreach (KeyValuePair<Rule, List<FileMapper>> item in this)
        {
            print.Information($"Total of [Cyan]{item.Value.Count} files match to rule [Cyan]{item.Key.Name}.");
        }
    }

    public AnalyzersAndFixers FindByKey(string rule)
    {
        return new AnalyzersAndFixers(this.Where(c => c.Key.Name == rule), this.print);
    }
}
