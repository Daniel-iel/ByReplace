using ByReplace.Specification.Conditions;

namespace ByReplace.Specification.Matches;
internal sealed class FileWithFolderSpec : IMatchSpec
{
    private readonly string _path;
    public FileWithFolderSpec(string path)
    {
        _path = path;
    }
    public bool IsSatisfiedBy(Rule rule)
    {
        foreach (var skip in rule.Skip)
        {
            if (_path.IndexOf(skip, StringComparison.InvariantCultureIgnoreCase) > -1)
            {
                return true;
            }
        }

        return false;
    }
}