using ByReplace.Commands.Command;
using Moq;
using Xunit;

namespace ByReplace.Test.Commands.Command;

public class CompositeCommandTest
{
    private readonly Mock<ICommand> _firstCommand;
    private readonly Mock<ICommand> _secondCommand;

    public CompositeCommandTest()
    {
        _firstCommand = new Mock<ICommand>();
        _secondCommand = new Mock<ICommand>();
    }

    [Fact]
    public async Task Execute_ExecuteCompositeCommands_ShouldValidateHowManyTimeCommandsIsExecuted()
    {
        // Arrange
        _firstCommand
            .Setup(x => x.ExecuteAsync(It.IsAny<CancellationToken>()))
            .Returns(ValueTask.CompletedTask);

        _secondCommand
            .Setup(x => x.ExecuteAsync(It.IsAny<CancellationToken>()))
            .Returns(ValueTask.CompletedTask);

        var command = new CompositeCommand(_firstCommand.Object, _secondCommand.Object);

        // Act
        await command.ExecuteAsync();

        // Assert
        _firstCommand.Verify(c => c.ExecuteAsync(It.IsAny<CancellationToken>()), Times.Once);
        _secondCommand.Verify(c => c.ExecuteAsync(It.IsAny<CancellationToken>()), Times.Once);
    }
}
