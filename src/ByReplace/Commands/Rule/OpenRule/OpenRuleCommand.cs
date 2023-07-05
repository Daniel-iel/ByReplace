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
        Models.Rule rule = configuration
            .Rules
            .Where(c => c.Name.Equals(ruleName.Trim(), StringComparison.InvariantCulture))
            .FirstOrDefault();

        if (rule is null)
        {
            print.PrintWarning($"Rule named {ruleName} was not found on brconfig file");

            return ValueTask.CompletedTask;
        }

        PrintRuleBuilder builder = new PrintRuleBuilder(rule);

        PrintBox printer = new PrintBox();
        printer.CreateBoxAndPrint(builder);

        return ValueTask.CompletedTask;
    }
}
