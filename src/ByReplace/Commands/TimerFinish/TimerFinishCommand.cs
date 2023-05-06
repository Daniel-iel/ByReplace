namespace ByReplace.Commands.TimerFinish
{
    internal class TimerFinishCommand : ICommand
    {
        private readonly IPrint print;

        public TimerFinishCommand(IPrint print)
        {
            this.print = print;
        }

        public ValueTask ExecuteAsync(CancellationToken cancellationToken = default)
        {
            print.InformationTimer();

            return ValueTask.CompletedTask;
        }
    }
}
