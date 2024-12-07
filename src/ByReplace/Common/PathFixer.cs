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

    public char GetPathSeparatior()
    {
        IPathFixer linux = new PathFixerLinux();
        IPathFixer windows = new PathFixerWindows();

        return RuntimeInformation.IsOSPlatform(OSPlatform.Linux)
           ? linux.Separator
           : windows.Separator;
    }

    public string[] GetPathParts(string path)
    {
        string pathFixed = GetFixedPath(path);
        return pathFixed.Split(GetPathSeparatior());
    }
}
