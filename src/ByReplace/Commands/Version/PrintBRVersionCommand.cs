namespace ByReplace.Commands.Version;

internal class PrintBRVersionCommand : ICommand
{
    public async ValueTask ExecuteAsync(CancellationToken cancellationToken = default)
    {
        NugetVersion nugetVersion = new NugetVersion();
        await nugetVersion.GetByReplaceNugetVersionAsync();
    }
}
