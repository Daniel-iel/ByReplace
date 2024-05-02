namespace ByReplace.Commands.Rule.ListRules;

internal sealed class RulesBox : IBox, IEquatable<RulesBox>, IEqualityComparer<RulesBox>
{
    private readonly ImmutableList<Models.Rule> _rules;

    public RulesBox(ImmutableList<Models.Rule> rules)
    {
        this.Width = 100;
        this.Height = rules.Count * 2;

        _rules = rules;
    }

    public int Width { get; }
    public int Height { get; }

    public string BoxName => "Rules";

    public string GetValuesToPrint()
    {
        StringBuilder rules = new StringBuilder();

        foreach (Models.Rule rule in _rules)
        {
            rules.AppendLine($"{rule.Name}: {rule.Description}");
        }

        return rules.ToString();
    }

    public bool Equals(RulesBox other)
    {
        return Width == other.Width &&
               Height == other.Height &&
               BoxName == other.BoxName;
    }

    public override bool Equals(object obj)
    {
        RulesBox other = (RulesBox)obj;

        return other.Equals(this);
    }

    public bool Equals(RulesBox x, RulesBox y)
    {
        return x.Equals(y);
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(Width, Height, BoxName);
    }

    public int GetHashCode([DisallowNull] RulesBox obj)
    {
        return HashCode.Combine(obj.Width, obj.Height, obj.BoxName);
    }
}