namespace ByReplace.Specification.Match;

internal class SkipMatchSpecification : IMatchSpecification
{
    private readonly string _directory;

    public SkipMatchSpecification(string directory)
    {
        _directory = directory;
    }

    public bool IsSatisfiedBy(FileMapper file, Rule rule)
    {
        return SkipFile(file, rule) || SkipDir(rule) || SkipDirWithFile(file, rule);
    }

    private bool SkipDir(Rule rule)
    {
        return rule.Skip.Any(c =>
                c.StartsWith("**", StringComparison.Ordinal) &&
                c.EndsWith("*", StringComparison.Ordinal) &&
                _directory.Contains(SanitizePattern(c), StringComparison.InvariantCultureIgnoreCase));
    }

    private static bool SkipFile(FileMapper file, Rule rule)
    {
        return rule.Skip.Any(c => c.EndsWith(file.Name, StringComparison.InvariantCultureIgnoreCase));
    }

    private static bool SkipDirWithFile(FileMapper file, Rule rule)
    {
        return rule.Skip.Any(c => file.FullName.EndsWith(c, StringComparison.InvariantCultureIgnoreCase));
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
