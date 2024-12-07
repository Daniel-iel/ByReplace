namespace ByReplace.Test.Common
{
    public class PathFixerLinuxTest
    {
        [Fact]
        public void GetFixedPath_ShouldReturnWindowsPath_WhenRunningOnLinux()
        {
            // Arrange
            var pathFixer = new PathFixerLinux();
            var parts = new[] { "folder", "subfolder", "file.txt" };

            // Act
            var path = pathFixer.PathFixed(parts);

            // Assert
            Assert.Equal(@"folder/subfolder/file.txt", path);
        }
    }
}
