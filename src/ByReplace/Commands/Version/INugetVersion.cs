// Ignore Spelling: Nuget

namespace ByReplace.Commands.Version;

internal interface INugetVersion
{
    Task<string> GetByReplaceNugetVersionAsync(CancellationToken cancellationToken);
}
