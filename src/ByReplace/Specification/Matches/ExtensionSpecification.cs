namespace ByReplace.Specification.Match;

internal class ExtensionSpecification : IMatchSpecification
{
    public bool IsSatisfiedBy(FileMapper file, Rule rule)
    {
        return rule.Extensions.Contains(file.Extension);
    }
}
