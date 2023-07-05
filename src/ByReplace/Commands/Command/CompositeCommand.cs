namespace ByReplace.Commands.Command;

internal class CompositeCommand : ICommand
{
    private readonly ICommand[] commands;

    public CompositeCommand(params ICommand[] commands)
    {
        this.commands = commands;
    }

    public async ValueTask ExecuteAsync(CancellationToken cancellationToken = default)
    {
        foreach (ICommand command in commands)
        {
            await command.ExecuteAsync(cancellationToken);
        }
    }
}
