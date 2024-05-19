
namespace ByReplace.Matches;

internal sealed class ExtensionMatch : Match
{
    private readonly string extension;
    private readonly string[] param;

    public ExtensionMatch(string extension, string[] param)
    {
        this.extension = extension;
        this.param = param;
    }
    public override bool HasMatch
        => param.Contains(extension);
}
