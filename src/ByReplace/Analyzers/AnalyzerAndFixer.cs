using ByReplace.Specification.Match;

[assembly: InternalsVisibleTo("ByReplace.Test")]

namespace ByReplace.Analyzers;

internal sealed class AnalyzerAndFixer : Dictionary<FileMapper, List<Rule>>
{
    private readonly IPrint _print;
    private readonly ImmutableList<Rule> _rules;

    public AnalyzerAndFixer(IPrint print, ImmutableList<Rule> rules) : this([], print)
    {
        _print = print;
        _rules = rules;
    }

    public AnalyzerAndFixer(IEnumerable<KeyValuePair<FileMapper, List<Rule>>> values, IPrint print) : base(values)
    {
        _print = print;
    }

    internal bool TryMatchRule(DirectoryNode directoryNode)
    {
        var skipSpec = new SkipMatchSpecification(directoryNode.Directory);
        var extensionSpec = new ExtensionSpecification();

        foreach (FileMapper file in directoryNode.Files)
        {
            foreach (Rule rule in _rules)
            {
                if (skipSpec.IsSatisfiedBy(file, rule) || !extensionSpec.IsSatisfiedBy(file, rule))
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

        foreach (KeyValuePair<FileMapper, List<Rule>> item in this)
        {
            _print.Information($"[Cyan]{item.Value.Count} rules in total match the file [Cyan]{item.Key.Name}.");
        }

        return false;
    }

    public AnalyzerAndFixer FindByKey(string rule)
    {
        return new AnalyzerAndFixer(this.Where(c => c.Key.Name == rule), this._print);
    }

    public AnalyzerAndFixer FindByRule(string rule)
    {
        var filteredDictionary = this.Where(entry => entry.Value.Any(r => r.Name == rule))
                                     .ToDictionary(entry => entry.Key, entry => entry.Value);

        return new AnalyzerAndFixer(filteredDictionary, _print);
    }
}
