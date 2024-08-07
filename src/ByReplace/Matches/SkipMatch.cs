﻿namespace ByReplace.Matches;

internal sealed class SkipMatch : Match
{
    private readonly string dir;
    private readonly FileMapper file;
    private readonly string[] param;

    public SkipMatch(string dir, FileMapper file, string[] param)
    {
        this.dir = dir;
        this.file = file;
        this.param = param;
    }

    public override bool HasMatch
    {
        get
        {
            return SkipFile() || (SkipDir() || SkipDirWithFile());
        }
    }

    private bool SkipFile()
    {
        return param.Any(c => c.EndsWith(file.Name, StringComparison.InvariantCultureIgnoreCase));
    }

    private bool SkipDirWithFile()
    {
        return param.Any(c => file.FullName.EndsWith(c, StringComparison.InvariantCultureIgnoreCase));
    }

    private bool SkipDir()
    {
        return param.Any(c =>
                c.StartsWith("**", StringComparison.Ordinal) &&
                c.EndsWith("*", StringComparison.Ordinal) &&
                 dir.Contains(SanitizePattern(c), StringComparison.InvariantCultureIgnoreCase));
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
