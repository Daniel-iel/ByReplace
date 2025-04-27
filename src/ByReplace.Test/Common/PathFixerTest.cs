using ByReplace.Common;
using ByReplace.Test.TestHelpers.Attributes;
using Xunit;

namespace ByReplace.Test.Common;

public class PathFixerTests
{
    [PlatformSpecificFact(Platform.Windows)]
    public void GetFixedPath_ShouldReturnWindowsPath_WhenRunningOnWindows()
    {
        // Arrange
        var pathFixer = new PathFixer();
        var parts = new[] { "folder", "subfolder", "file.txt" };

        // Act
        var path = pathFixer.GetFixedPath(parts);

        // Assert
        Assert.Equal(@"folder\subfolder\file.txt", path);
    }

    [PlatformSpecificFact(Platform.Linux)]
    public void GetFixedPath_ShouldReturnLinuxPath_WhenRunningOnLinux()
    {
        // Arrange
        var pathFixer = new PathFixer();
        var parts = new[] { "folder", "subfolder", "file.txt" };

        // Act
        var path = pathFixer.GetFixedPath(parts);

        // Assert
        Assert.Equal("folder/subfolder/file.txt", path);
    }
}
