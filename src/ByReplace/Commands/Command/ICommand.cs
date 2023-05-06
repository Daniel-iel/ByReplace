namespace ByReplace.Commands.Command;

internal interface ICommand
{
    public ValueTask ExecuteAsync(CancellationToken cancellationToken = default);
}
