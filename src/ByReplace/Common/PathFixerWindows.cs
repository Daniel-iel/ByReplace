namespace ByReplace.Common;

internal sealed class PathFixerWindows : IPathFixer
{
    public char Separator => '\\';

    public string PathFixed(params string[] parts)
    {
        return string
            .Join('\\', parts)
            .Trim();
    }
}
