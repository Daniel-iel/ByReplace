namespace ByReplace.Common;

internal sealed class PathFixerWindows : IPathFixer
{
    public string PathFixed(params string[] parts)
    {
        return string
            .Join('\\', parts)
            .Trim();
    }
}
