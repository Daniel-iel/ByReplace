using System.Runtime.InteropServices;

namespace ByReplace.Common;

internal class PathFixer
{
    public string OS
    {
        get
        {
            return Environment.OSVersion.ToString();
        }
    }

    public string GetFixedPath(params string[] parts)
    {
        IPathFixer linux = new PathFixerLinux();
        IPathFixer windows = new PathFixerLinux();

        return RuntimeInformation.IsOSPlatform(OSPlatform.Linux)
            ? linux.PathFixed(parts)
            : windows.PathFixed(parts);
    }
}

internal interface IPathFixer
{
    string PathFixed(params string[] parts);
}

internal class PathFixerWindowns : IPathFixer
{
    public string PathFixed(params string[] parts)
    {
        return string
            .Join('\\', parts)
            .Trim();
    }
}

internal class PathFixerLinux : IPathFixer
{
    public string PathFixed(params string[] parts)
    {
        return string
            .Join('/', parts)
            .Trim();
    }
}