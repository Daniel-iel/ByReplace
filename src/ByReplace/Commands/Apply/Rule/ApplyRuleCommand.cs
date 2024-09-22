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
        var sourceThreeProvider = new SourceThreeProvider(configuration, print);
        var matchProvider = new MatchProvider(configuration, print, sourceThreeProvider);
        var documentFix = new DocumentFixProvider(print, matchProvider);

        return documentFix.RunAsync(applyRuleParameter.Rule, cancellationToken);
    }
}
