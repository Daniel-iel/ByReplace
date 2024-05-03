namespace ByReplace.Commands.TimerFinish;

internal sealed class TimerFinishCommand : ICommand
{
    private readonly IPrint print;

    public TimerFinishCommand(IPrint print)
    {
        this.print = print;
    }

    public ValueTask ExecuteAsync(CancellationToken cancellationToken = default)
    {
        print.Timer();

        return ValueTask.CompletedTask;
    }
}
