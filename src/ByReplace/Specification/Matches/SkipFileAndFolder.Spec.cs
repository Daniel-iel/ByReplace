namespace ByReplace.Specification.Match;

internal sealed class SkipFileAndFolderSpec : IMatchSpec
{
    private readonly string[] _pathParts;

    public SkipFileAndFolderSpec(string[] pathParts)
    {
        _pathParts = pathParts;
    }

    public bool IsSatisfiedBy(Rule rule)
    {
        ReadOnlySpan<string> pathPartsAsSpan = new Span<string>(_pathParts);

        return SkipFile(rule.Skip, pathPartsAsSpan) || SkipFileWithFoder(rule.Skip, pathPartsAsSpan);
    }

    private static bool SkipFile(string[] skips, ReadOnlySpan<string> pathParts)
    {
        var fileName = pathParts[^1..];

        foreach (var skip in skips)
        {
            if (skip.Equals(fileName.ToString(), StringComparison.InvariantCultureIgnoreCase))
            {
                return true;
            }
        }

        return false;
    }

    private static bool SkipFileWithFoder(string[] skips, ReadOnlySpan<string> pathParts)
    {
        var fileNameWithParentFolder = pathParts[^2..];

        foreach (var skip in skips)
        {
            if (skip.Equals(fileNameWithParentFolder.ToString(), StringComparison.InvariantCultureIgnoreCase))
            {
                return true;
            }
        }

        return false;
    }
}
