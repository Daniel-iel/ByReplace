namespace ByReplace.Commands.Rule.OpenRule;

internal sealed class OpenRuleCommand : ICommand
{
    private readonly BrConfiguration configuration;
    private readonly string ruleName;
    private readonly IPrint print;
    private readonly IPrintBox printBox;

    public OpenRuleCommand(BrConfiguration configuration, string ruleName, IPrint print, IPrintBox printBox)
    {
        this.configuration = configuration;
        this.ruleName = ruleName;
        this.print = print;
        this.printBox = printBox;
    }

    public ValueTask ExecuteAsync(CancellationToken cancellationToken = default)
    {
        Models.Rule rule = configuration
            .Rules
            .FirstOrDefault(c => c.Name.Equals(ruleName.Trim(), StringComparison.CurrentCultureIgnoreCase));

        if (rule is null)
        {
            print.Warning($"Rule named {ruleName} was not found on brconfig file");

            return ValueTask.CompletedTask;
        }

        RuleBox builder = new RuleBox(rule);

        printBox.CreateBoxAndPrint(builder);

        return ValueTask.CompletedTask;
    }
}
