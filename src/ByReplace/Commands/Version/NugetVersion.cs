﻿using ByReplace.Printers;
using NuGet.Configuration;
using System.Reflection;
namespace ByReplace.Commands.Version;

internal class NugetVersion
{
    private readonly SourceRepository _sourceRepository;
    private readonly SourceCacheContext _sourceCacheContext;
    private readonly PrintConsole _printConsole;

    public NugetVersion()
    {
        _sourceRepository = Repository.Factory.GetCoreV3(new PackageSource(NuGetConstants.V3FeedUrl));
        _sourceCacheContext = new SourceCacheContext();
        _printConsole = new PrintConsole();
    }

    public async Task<string> GetByReplaceNugetVersionAsync()
    {
        var assembly = Assembly.GetExecutingAssembly();
        var version = assembly.GetCustomAttribute<AssemblyInformationalVersionAttribute>()?.InformationalVersion;

        SemanticVersion.TryParse(version, out var currentVersion);

        _printConsole.PrintInfo($"Version: [Green]{currentVersion}.");

        var latestVersion = await GetVersionAsync(preRelease: false);
        if (latestVersion > currentVersion)
        {
            _printConsole.PrintInfo($"A new version of ByReplace [Yellow]({latestVersion}) is available. Please consider upgrading using the command `dotnet tool update -g ByReplace`");
        }
        else
        {
            var previewVersion = await GetVersionAsync(preRelease: true);
            if (previewVersion > currentVersion)
            {
                _printConsole.PrintInfo($@"A preview version of ByReplace [Yellow]({previewVersion}) is available on nuget.
If you would like to try out this preview version you can install it with `dotnet tool update -g ByReplace --version {previewVersion}`
Since this is a preview feature things might not work as expected! Please report any findings on GitHub![/]");
            }
        }

        return currentVersion.ToString();
    }

    private async Task<SemanticVersion> GetVersionAsync(bool preRelease)
    {
        try
        {
            var metadataResource = await _sourceRepository.GetResourceAsync<MetadataResource>();
            var versionsFound = await metadataResource
                 .GetVersions("ByReplace", includePrerelease: true, includeUnlisted: false, _sourceCacheContext, null, CancellationToken.None);

            return versionsFound
                 .OrderBy(x => x)
                 .Last(x => preRelease ? x.IsPrerelease : !x.IsPrerelease);
        }
        catch
        {
            return new SemanticVersion(0, 0, 0);
        }
    }
}