using ByReplace.Commands.Command;

namespace ByReplace.Commands.Version;

internal class PrintBRVersionCommand : ICommand
{
    public async ValueTask ExecuteAsync(CancellationToken cancellationToken = default)
    {
        var nugetVersion = new NugetVersion();
        await nugetVersion.GetByReplaceNugetVersionAsync();
    }
}
