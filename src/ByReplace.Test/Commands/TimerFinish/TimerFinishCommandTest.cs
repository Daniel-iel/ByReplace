using ByReplace.Commands.TimerFinish;
using ByReplace.Printers;
using Moq;
using Xunit;

namespace ByReplace.Test.Commands.TimerFinish;

public class TimerFinishCommandTest
{
    private readonly Mock<IPrint> _printMock;

    public TimerFinishCommandTest()
    {
        _printMock = new Mock<IPrint>();
    }

    [Fact]
    public async Task ExecuteAsync_ShouldCallPrintTimerOnce_WhenExecutionFinishes()
    {
        // Arrange
        var command = new TimerFinishCommand(_printMock.Object);

        // Act
        await command.ExecuteAsync();

        // Assert
        _printMock.Verify(c => c.Timer(), Times.Once);
    }
}
