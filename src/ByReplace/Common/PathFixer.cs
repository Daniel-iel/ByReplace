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
