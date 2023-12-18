namespace ByReplace.Matches;

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
            return SkipFile() && SkipDir();
        }
    }

    private bool SkipFile()
    {
        return param.Contains(file.Name);
    }

    private bool SkipDir()
    {
        return param.Any(c =>
                c.StartsWith("**", StringComparison.Ordinal) &&
                c.EndsWith("*",StringComparison.Ordinal) &&
                dir.Contains(c));
    }
}
