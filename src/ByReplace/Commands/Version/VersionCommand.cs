namespace ByReplace.Commands.Version;

internal class VersionCommand : ICommand
{
    private readonly INugetVersion _nugetVersion;

    public VersionCommand(INugetVersion nugetVersion)
    {
        _nugetVersion = nugetVersion;
    }

    public async ValueTask ExecuteAsync(CancellationToken cancellationToken)
    {
        await _nugetVersion.GetByReplaceNugetVersionAsync(cancellationToken);
    }
}
