namespace ByReplace.Specification.Match;

internal interface IMatchSpecification
{
    bool IsSatisfiedBy(FileMapper file, Rule rule);
}
