using ByReplace.Providers;

[assembly: InternalsVisibleTo("ByReplace.Test")]

namespace ByReplace.Commands.Apply.Rule;

internal sealed class ApplyRuleCommand : ICommand
{
    private readonly BrConfiguration configuration;
    private readonly ApplyRuleParameter applyRuleParameter;
    private readonly IPrint print;

    public ApplyRuleCommand(
        BrConfiguration configuration,
        ApplyRuleParameter applyRuleParameter,
        IPrint print)
    {
        this.configuration = configuration;
        this.applyRuleParameter = applyRuleParameter;
        this.print = print;
    }

    public ValueTask ExecuteAsync(CancellationToken cancellationToken = default)
    {
        var documentFix = new DocumentFixProvider(
            print,
            new MatchProvider(configuration, print,
            new SourceThreeProvider(configuration, print)));

        return documentFix.RunAsync(applyRuleParameter.Rule, cancellationToken);
    }
}
