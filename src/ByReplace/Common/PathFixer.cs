using System.Runtime.InteropServices;

namespace ByReplace.Common;

[ExcludeFromCodeCoverage(Justification = "This test cases is converge but for mutant text will not possible cover, because I use win.")]
internal class PathFixer
{
    readonly IPathFixer linux = new PathFixerLinux();
    readonly IPathFixer windows = new PathFixerWindows();

    public string GetFixedPath(params string[] parts)
    {
        return RuntimeInformation.IsOSPlatform(OSPlatform.Linux)
            ? linux.PathFixed(parts)
            : windows.PathFixed(parts);
    }

    public char GetPathSeparator()
    {
        return RuntimeInformation.IsOSPlatform(OSPlatform.Linux)
           ? linux.Separator
           : windows.Separator;
    }

    public string[] GetPathParts(string path)
    {
        string pathFixed = GetFixedPath(path);
        return pathFixed.Split(GetPathSeparator());
    }
}
