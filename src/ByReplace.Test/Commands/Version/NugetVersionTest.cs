// Ignore Spelling: Nuget

using ByReplace.Commands.TimerFinish;
using ByReplace.Printers;
using Moq;
using Xunit;

namespace ByReplace.Test.Commands.Version
{
    public class NugetVersionTest
    {
        public readonly Mock<IPrint> _printMock;

        public NugetVersionTest()
        {
            _printMock = new Mock<IPrint>();
        }

        [Fact]
        public async Task Execute_WhenCalledTimerToPrintTimeOfExecution_ShouldVerifyIfTimerWasCalledOnce()
        {
            // Arrange
            var command = new TimerFinishCommand(_printMock.Object);

            // Act
            await command.ExecuteAsync(It.IsAny<CancellationToken>());

            // Assert
            _printMock.Verify(c => c.Timer(), Times.Once);
        }
    }
}
