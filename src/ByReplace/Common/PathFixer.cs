using System.Runtime.InteropServices;

namespace ByReplace.Common;

internal class PathFixer
{
    public string GetFixedPath(params string[] parts)
    {
        IPathFixer linux = new PathFixerLinux();
        IPathFixer windows = new PathFixerWindows();

        return RuntimeInformation.IsOSPlatform(OSPlatform.Linux)
            ? linux.PathFixed(parts)
            : windows.PathFixed(parts);
    }
}

internal interface IPathFixer
{
    string PathFixed(params string[] parts);
}

internal sealed class PathFixerWindows : IPathFixer
{
    public string PathFixed(params string[] parts)
    {
        return string
            .Join('\\', parts)
            .Trim();
    }
}

internal sealed class PathFixerLinux : IPathFixer
{
    public string PathFixed(params string[] parts)
    {
        return string
            .Join('/', parts)
            .Trim();
    }
}