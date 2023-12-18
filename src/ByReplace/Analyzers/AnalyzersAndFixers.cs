[assembly: InternalsVisibleTo("ByReplace.Test")]

namespace ByReplace.Analyzers;

internal class AnalyzersAndFixers : Dictionary<FileMapper, List<Rule>>
{
    private readonly IPrint print;

    public AnalyzersAndFixers(IPrint print) : this(Enumerable.Empty<KeyValuePair<FileMapper, List<Rule>>>(), print)
    {
    }

    public AnalyzersAndFixers(IEnumerable<KeyValuePair<FileMapper, List<Rule>>> values, IPrint print) : base(values)
    {
        this.print = print;
    }

    public bool TryMatchRole(DirectoryNode directoryNode, ImmutableList<Rule> rules)
    {
        foreach (FileMapper file in directoryNode.Files)
        {
            foreach (Rule rule in rules)
            {
                Match skipDirMatch = new SkipMatch(directoryNode.Directory, file, rule.Skip);
                Match extensionMatch = new ExtensionMatch(file.Extension, rule.Extensions);

                if (skipDirMatch.HasMatch || !extensionMatch.HasMatch)
                {
                    continue;
                }

                if (!this.ContainsKey(file))
                {
                    this.Add(file, new List<Rule>());
                }

                this[file].Add(rule);
            }
        }

        Print();

        return false;
    }

    private void Print()
    {
        foreach (KeyValuePair<FileMapper, List<Rule>> item in this)
        {
            print.Information($"[Cyan]{item.Value.Count} rules in total match the file [Cyan]{item.Key.Name}.");
        }
    }

    public AnalyzersAndFixers FindByKey(string rule)
    {
        return new AnalyzersAndFixers(this.Where(c => c.Key.Name == rule), this.print);
    }
}
