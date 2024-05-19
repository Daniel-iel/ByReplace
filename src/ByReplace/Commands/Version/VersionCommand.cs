// Ignore Spelling: nuget

namespace ByReplace.Commands.Version;

internal sealed class VersionCommand : ICommand
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
