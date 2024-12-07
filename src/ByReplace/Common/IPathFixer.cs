namespace ByReplace.Common;

internal interface IPathFixer
{
    public char Separator { get; }

    string PathFixed(params string[] parts);
}
