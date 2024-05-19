namespace ByReplace.Commands.Rule.OpenRule;

internal sealed class RuleBox : IBox, IEquatable<RuleBox>, IEqualityComparer<RuleBox>
{
    private readonly Models.Rule _rule;

    public RuleBox(Models.Rule rule)
    {
        _rule = rule;

        BoxName = "Rule";
        Width = 100;
        Height = 5 * 2;
    }

    public string BoxName { get; }

    public int Width { get; }

    public int Height { get; }

    public string GetValuesToPrint()
    {
        StringBuilder sb = new StringBuilder(5);

        sb.AppendLine($"Name: {_rule.Name}");
        sb.AppendLine($"Description: {_rule.Description}");
        sb.AppendLine($"Skip: [ {_rule.Skip.Aggregate((a, b) => $"{a}, {b}")} ]");
        sb.AppendLine($"Extensions: [ {_rule.Extensions.Aggregate((a, b) => $"{a}, {b}")} ]");
        sb.AppendLine($"Replacement: FROM [ {_rule.Replacement.Old.Aggregate((a, b) => $"{a}, {b}")} ] To [ {_rule.Replacement.New} ]");

        return sb.ToString();
    }

    public bool Equals(RuleBox other)
    {
        return Width == other.Width &&
               Height == other.Height &&
               BoxName == other.BoxName &&
               _rule == other._rule;
    }

    public override bool Equals(object obj)
    {
        RuleBox other = (RuleBox)obj;

        return other.Equals(this);
    }

    public bool Equals(RuleBox x, RuleBox y)
    {
        return x.Equals(y);
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(Width, Height, BoxName, _rule);
    }

    public int GetHashCode([DisallowNull] RuleBox obj)
    {
        return HashCode.Combine(obj.Width, obj.Height, obj.BoxName, obj._rule);
    }
}