namespace ByReplace.Commands.Handlers
{
    internal interface ICommand
    {
        public ValueTask ExecuteAsync(CancellationToken cancellationToken = default);
    }
}
