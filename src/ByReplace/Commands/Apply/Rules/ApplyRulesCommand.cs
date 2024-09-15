using ByReplace.Providers;

[assembly: InternalsVisibleTo("ByReplace.Test")]

namespace ByReplace.Commands.Apply.Rules;

internal sealed class ApplyRulesCommand : ICommand
{
    private readonly BrConfiguration configuration;
    private readonly IPrint print;

    public ApplyRulesCommand(BrConfiguration configuration, IPrint print)
    {
        this.configuration = configuration;
        this.print = print;
    }

    public ValueTask ExecuteAsync(CancellationToken cancellationToken = default)
    {
        var documentFix = new DocumentFixProvider(
                  print,
                  new MatchProvider(configuration, print,
                  new SourceThreeProvider(configuration, print)));

        return documentFix.RunAsync(cancellationToken);
    }
}
