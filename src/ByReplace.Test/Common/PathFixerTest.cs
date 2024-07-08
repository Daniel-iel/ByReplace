using System.Runtime.InteropServices;
using Xunit;

namespace ByReplace.Common.Tests;

public class PathFixerTests
{
    [Fact]
    public void GetFixedPath_ShouldReturnWindowsPath_WhenRunningOnWindows()
    {
        // Arrange
        var pathFixer = new PathFixer();
        var parts = new[] { "folder", "subfolder", "file.txt" };

        // Act
        var path = pathFixer.GetFixedPath(parts);

        // Assert
        if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
        {
            Assert.Equal(@"folder\subfolder\file.txt", path);
        }
    }

    [Fact]
    public void GetFixedPath_ShouldReturnLinuxPath_WhenRunningOnLinux()
    {
        // Arrange
        var pathFixer = new PathFixer();
        var parts = new[] { "folder", "subfolder", "file.txt" };

        // Act
        var path = pathFixer.GetFixedPath(parts);

        // Assert
        if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
        {
            Assert.Equal("folder/subfolder/file.txt", path);
        }
    }
}
