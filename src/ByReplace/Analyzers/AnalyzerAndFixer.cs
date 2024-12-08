using ByReplace.Common;
using ByReplace.Specification.Matches;
using static ByReplace.Mappers.DirectoryThreeV2;

[assembly: InternalsVisibleTo("ByReplace.Test")]

namespace ByReplace.Analyzers;

internal sealed class AnalyzerAndFixer : System.Collections.Concurrent.ConcurrentDictionary<SourceThree, List<Rule>>
{
    private readonly IPrint _print;
    private readonly ImmutableList<Rule> _rules;

    public AnalyzerAndFixer(IPrint print, ImmutableList<Rule> rules) : this([], print)
    {
        _print = print;
        _rules = rules;
    }

    private AnalyzerAndFixer(IEnumerable<KeyValuePair<SourceThree, List<Rule>>> values, IPrint print) : base(values)
    {
        _print = print;
    }

    [MethodImpl(MethodImplOptions.AggressiveOptimization)]
    internal void TryMatchRule(SourceThree sourceThree)
    {
        PathFixer path = new PathFixer();
        var pathParts = path.GetPathParts(sourceThree.Path);

        SkipDirectorySpec skipDirectorySpec = new(pathParts);
        SkipFileAndFolderSpec skipFileAndFolderSpec = new(pathParts);
        SkipExtensionSpec skipExtensionSpec = new(pathParts);

        foreach (var rule in _rules)
        {
            if (skipDirectorySpec.IsSatisfiedBy(rule) ||
                skipFileAndFolderSpec.IsSatisfiedBy(rule) ||
                skipExtensionSpec.IsSatisfiedBy(rule))
            {
                continue;
            }

            if (!this.ContainsKey(sourceThree))
            {
                this.TryAdd(sourceThree, new List<Rule>());
            }

            this[sourceThree].Add(rule);
        }

        foreach (var item in this)
        {
            var fileInformartion = new FileInfo(item.Key.Path);

            _print.Information($"[Cyan]{item.Value.Count} rules in total match the file [Cyan]{fileInformartion.Name}.");
        }
    }

    public AnalyzerAndFixer FindByRule(string rule)
    {
        var filteredDictionary = this.Where(entry => entry.Value.Any(r => r.Name == rule))
                                     .ToDictionary(entry => entry.Key, entry => entry.Value);

        return new AnalyzerAndFixer(filteredDictionary, _print);
    }
}
