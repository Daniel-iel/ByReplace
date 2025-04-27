using ByReplace.Specification.Conditions;

namespace ByReplace.Specification.Matches;

internal sealed class ExtensionSpec : IMatchSpec
{
    private readonly string[] _pathsParts;

    public ExtensionSpec(string[] pathsParts)
    {
        _pathsParts = pathsParts;
    }

    public bool IsSatisfiedBy(Rule rule)
    {
        var file = _pathsParts[^1..][0];

        if (!file.Contains('.'))
        {
            return false;
        }

        // .text, .cs, .json
        foreach (var pathPart in rule.Extensions)
        {
            if (file.IndexOf(pathPart, StringComparison.InvariantCultureIgnoreCase) > -1)
            {
                return true;
            }
        }

        return false;
    }
}
