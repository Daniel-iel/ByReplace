
using ByReplace.Commands.Handlers;

namespace ByReplace.Commands.Logo
{
    internal class PrintLogoCommand : ICommand
    {
        public ValueTask ExecuteAsync(CancellationToken cancellationToken = default)
        {
            Console.WriteLine("Comming soon!");

            return new ValueTask();
        }
    }
}
