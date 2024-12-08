namespace ByReplace.Specification.Conditions;

internal sealed class OrSpecification : IMatchSpec
{
    private readonly IMatchSpec leftSpec;
    private readonly IMatchSpec rightSpec;

    public OrSpecification(IMatchSpec leftSpec, IMatchSpec rightSpec)
    {
        this.leftSpec = leftSpec;
        this.rightSpec = rightSpec;
    }

    public bool IsSatisfiedBy(Rule rule)
    {
        return leftSpec.IsSatisfiedBy(rule) || rightSpec.IsSatisfiedBy(rule);
    }
}
