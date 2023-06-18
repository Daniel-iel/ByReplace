namespace ByReplace.Commands.Rule.ListRules;

internal class ListRulesCommand : ICommand
{
    private readonly BrConfiguration configuration;
    private readonly IPrint print;

    public ListRulesCommand(BrConfiguration configuration, IPrint print)
    {
        this.configuration = configuration;
        this.print = print;
    }

    public ValueTask ExecuteAsync(CancellationToken cancellationToken = default)
    {
        PrintRulesBuilder builder = new PrintRulesBuilder(configuration.Rules);

        PrintBox printer = new PrintBox();
        printer.CreateBoxAndPrint(builder);

        return ValueTask.CompletedTask;
    }
}
