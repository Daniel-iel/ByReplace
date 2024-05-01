[assembly: InternalsVisibleTo("DynamicProxyGenAssembly2")]
[assembly: InternalsVisibleTo("ByReplace.Test")]

namespace ByReplace.Commands.Command;

internal interface ICommand
{
    public ValueTask ExecuteAsync(CancellationToken cancellationToken = default);
}
