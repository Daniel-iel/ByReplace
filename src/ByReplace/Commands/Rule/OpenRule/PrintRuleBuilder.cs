namespace ByReplace.Commands.Rule.OpenRule;

internal class PrintRuleBuilder : IPrintBox
{
    private readonly Models.Rule _rule;

    public PrintRuleBuilder(Models.Rule rule)
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
}