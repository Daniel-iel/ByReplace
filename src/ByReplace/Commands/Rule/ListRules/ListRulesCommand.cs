namespace ByReplace.Commands.Rule.ListRules;

internal class ListRulesCommand : ICommand
{
    private readonly BrConfiguration configuration;
    private readonly IPrintBox printBox;

    public ListRulesCommand(BrConfiguration configuration, IPrintBox printBox)
    {
        this.configuration = configuration;
        this.printBox = printBox;
    }

    public ValueTask ExecuteAsync(CancellationToken cancellationToken = default)
    {
        RulesBox rulesBox = new RulesBox(configuration.Rules);

        printBox.CreateBoxAndPrint(rulesBox);

        return ValueTask.CompletedTask;
    }
}
