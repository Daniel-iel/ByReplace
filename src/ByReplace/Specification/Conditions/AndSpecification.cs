namespace ByReplace.Specification.Match;

internal sealed class AndSpecification : IMatchSpecification
{
    private readonly IMatchSpecification leftSpec;
    private readonly IMatchSpecification rightSpec;

    public AndSpecification(IMatchSpecification leftSpec, IMatchSpecification rightSpec)
    {
        this.leftSpec = leftSpec;
        this.rightSpec = rightSpec;
    }

    public bool IsSatisfiedBy(FileMapper file, Rule rule)
    {
        return leftSpec.IsSatisfiedBy(file, rule) && rightSpec.IsSatisfiedBy(file, rule);
    }
}
