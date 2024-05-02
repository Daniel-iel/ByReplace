namespace ByReplace.Commands.Version;

internal class VersionCommand : ICommand
{
    public async ValueTask ExecuteAsync(CancellationToken cancellationToken)
    {
        NugetVersion nugetVersion = new NugetVersion();
        await nugetVersion.GetByReplaceNugetVersionAsync(cancellationToken);
    }
}
