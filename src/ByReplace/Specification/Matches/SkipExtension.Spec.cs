
namespace ByReplace.Specification.Match;

internal sealed class SkipExtensionSpec : IMatchSpec
{
    private readonly string[] _pathsParts;

    public SkipExtensionSpec(string[] pathsParts)
    {
        _pathsParts = pathsParts;
    }

    public bool IsSatisfiedBy(Rule rule)
    {
        var file = _pathsParts[^1..];

        if (!file.Contains("."))
        {
            return false;
        }

        var extension = file[0]
            .Split(".")
            .ToArray()[1];

        foreach (var pathPart in rule.Skip)
        {
            if (pathPart.Equals(extension, StringComparison.InvariantCultureIgnoreCase))
            {
                return true;
            }
        }

        return false;
    }
}
