namespace ByReplace.Common;

internal sealed class PathFixerLinux : IPathFixer
{
    public char Separator => '/';

    public string PathFixed(params string[] parts)
    {
        return string
            .Join('/', parts)
            .Trim();
    }
}