namespace ByReplace.Commands.Rule.OpenRule;

internal class OpenRuleCommand : ICommand
{
    private readonly BrConfiguration configuration;
    private readonly string ruleName;
    private readonly IPrint print;

    public OpenRuleCommand(BrConfiguration configuration, string ruleName, IPrint print)
    {
        this.configuration = configuration;
        this.ruleName = ruleName;
        this.print = print;
    }

    public ValueTask ExecuteAsync(CancellationToken cancellationToken = default)
    {
        var rule = configuration
            .Rules
            .Where(c => c.Name.Equals(ruleName.Trim(), StringComparison.InvariantCulture))
            .FirstOrDefault();

        if (rule is null)
        {
            print.PrintWarning($"Rule named {ruleName} was not found on brconfig file");

            return ValueTask.CompletedTask;
        }

        var printer = new PrintBox();
        printer.CreateBox("Rule", 100, 5 * 2);

        StringBuilder sb = new StringBuilder(5);

        sb.AppendLine($"Name: {rule.Name}");
        sb.AppendLine($"Description: {rule.Description}");
        sb.AppendLine($"Skip: [ {rule.Skip.Aggregate((a, b) => $"{a}, {b}")} ]");
        sb.AppendLine($"Extensions: [ {rule.Extensions.Aggregate((a, b) => $"{a}, {b}")} ]");
        sb.AppendLine($"Replacement: FROM [ {rule.Replacement.Old.Aggregate((a, b) => $"{a}, {b}")} ] To [ {rule.Replacement.New} ]");

        printer.Print(sb.ToString());


        return ValueTask.CompletedTask;
    }
}
