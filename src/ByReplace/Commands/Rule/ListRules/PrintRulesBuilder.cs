namespace ByReplace.Commands.Rule.ListRules;

internal class PrintRulesBuilder : IPrintBox
{
    private readonly ImmutableList<Models.Rule> _rules;

    public PrintRulesBuilder(ImmutableList<Models.Rule> rules)
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

        foreach (var rule in _rules)
        {
            rules.AppendLine($"{rule.Name}: {rule.Description}");
        }

        return rules.ToString();
    }
}