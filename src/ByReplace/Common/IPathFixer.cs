namespace ByReplace.Common;

internal interface IPathFixer
{
    string PathFixed(params string[] parts);
}
