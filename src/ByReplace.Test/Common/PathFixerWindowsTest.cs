using ByReplace.Common;
using Xunit;

namespace ByReplace.Test.Common
{
    public class PathFixerWindowsTest
    {
        [Fact]
        public void GetFixedPath_ShouldReturnWindowsPath_WhenRunningOnWindows()
        {
            // Arrange
            var pathFixer = new PathFixerWindows();
            var parts = new[] { "folder", "subfolder", "file.txt" };

            // Act
            var path = pathFixer.PathFixed(parts);

            // Assert
            Assert.Equal(@"folder\subfolder\file.txt", path);
        }
    }
}
