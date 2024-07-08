using ByReplace.Mappers;
using ByReplace.Matches;
using Xunit;

namespace ByReplace.Test.Matches
{
    public class SkipMatchTest
    {
        [Theory]
        [InlineData("testFile.txt", "c://byreplace//testFile.txt", ".txt", "c://byreplace//", "**/byreplace/*")]
        public void SkipMatch(
            string fileName,
            string fileFullPath,
            string fileExtension,
            string dirPath,
            params string[] rules)
        {
            // Arrange
            var fileMapperMock = new FileMapper(Guid.NewGuid(), fileName, fileFullPath, fileExtension);

            var match = new SkipMatch(dirPath, fileMapperMock, rules);

            // Act
            var hasMatch = match.HasMatch;

            // Assert
            Assert.True(hasMatch);
        }

        [Fact]
        public void HasMatch_WhenTryMatchTheFileAndItNotMatchToAnyRuleToSkip_ShouldReturnFalse()
        {
            // Arrange
            var fileMapperMock = new FileMapper(Guid.NewGuid(), "testFile.txt", "c://byreplace//testFile.txt", ".txt");

            var match = new SkipMatch("c://byreplace//", fileMapperMock, new string[] { "Startup.cs", "Program.cs", "**/Controllers/*" });

            // Act
            var hasMatch = match.HasMatch;

            // Assert
            Assert.False(hasMatch);
        }

        [Fact]
        public void HasMatch_WhenTryMatchSomeFileByDirectoryPath_ShouldReturnTrue()
        {
            // Arrange
            var fileMapperMock = new FileMapper(Guid.NewGuid(), "testFile.txt", "c://byreplace//testFile.txt", ".txt");

            var match = new SkipMatch("c://byreplace//", fileMapperMock, new string[] { "**/byreplace/*" });

            // Act
            var hasMatch = match.HasMatch;

            // Assert
            Assert.True(hasMatch);
        }

        [Fact]
        public void HasMatch_WhenTryMatchSomeFileByWrongDirectoryPath_ShouldReturnFalse()
        {
            // Arrange
            var fileMapperMock = new FileMapper(Guid.NewGuid(), "testFile.txt", "c://byreplace//testFile.txt", ".txt");

            var match = new SkipMatch("c://byreplace//", fileMapperMock, new string[] { "**/byreplace/" });

            // Act
            var hasMatch = match.HasMatch;

            // Assert
            Assert.False(hasMatch);
        }

        [Fact]
        public void HasMatch_WhenTryMatchSomeFileByDirectoryWithFileNamePath_ShouldReturnTrue()
        {
            // Arrange
            var fileMapperMock = new FileMapper(Guid.NewGuid(), "testFile.txt", "c://byreplace//testFile.txt", ".txt");

            var match = new SkipMatch("c://byreplace//", fileMapperMock, new string[] { "//byreplace//testFile.txt" });

            // Act
            var hasMatch = match.HasMatch;

            // Assert
            Assert.True(hasMatch);
        }
    }
}