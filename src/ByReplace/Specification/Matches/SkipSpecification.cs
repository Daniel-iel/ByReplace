namespace ByReplace.Specification.Match;

internal class SkipMatchSpecification : IMatchSpecification
{
    private readonly DirectoryNode directoryNode;

    //private readonly string dir;
    //private readonly FileMapper file;
    //private readonly string[] param;

    public SkipMatchSpecification(DirectoryNode directoryNode)
    {
        this.directoryNode = directoryNode;
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
                directoryNode.Directory.Contains(SanitizePattern(c), StringComparison.InvariantCultureIgnoreCase));
    }

    private bool SkipFile(FileMapper file, Rule rule)
    {
        return rule.Skip.Any(c => c.EndsWith(file.Name, StringComparison.InvariantCultureIgnoreCase));
    }

    private bool SkipDirWithFile(FileMapper file, Rule rule)
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
