namespace ByReplace.Specification.Match;

internal sealed class SkipDirectorySpec : IMatchSpec
{
    private readonly string[] _pathParts;

    public SkipDirectorySpec(string[] pathParts)
    {
        _pathParts = pathParts;
    }

    public bool IsSatisfiedBy(Rule rule)
    {
        foreach (var skip in rule.Skip)
        {
            if (skip.StartsWith("**", StringComparison.Ordinal) &&
                skip.EndsWith("*", StringComparison.Ordinal) &&
                ShouldSkip(SanitizePattern(skip)))
            {
                return true;
            }
        }

        return false;
    }

    private bool ShouldSkip(string skip)
    {
        return _pathParts.Any(c => c.Equals(skip, StringComparison.InvariantCultureIgnoreCase));
    }

    [MethodImpl(MethodImplOptions.AggressiveOptimization)]
    private static unsafe string SanitizePattern(string pattern)
    {
        int maxBufferSize = pattern.Length;
        char* buffer = stackalloc char[maxBufferSize];
        int index = 0;

        foreach (char c in pattern)
        {
            if (c is not '*' and not '\\' and not '/')
            {
                buffer[index++] = c;
            }
        }

        buffer[index] = '\0';

        return new string(buffer);
    }
}