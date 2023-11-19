namespace ByReplace.Commands.Version;

internal class PrintBRVersionCommand : ICommand
{
    public async ValueTask ExecuteAsync(CancellationToken cancellationToken)
    {
        NugetVersion nugetVersion = new NugetVersion();
        await nugetVersion.GetByReplaceNugetVersionAsync(cancellationToken);
    }
}
