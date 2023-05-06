using ByReplace.Builders;
using ByReplace.Commands.Apply.Rules;
using ByReplace.Commands.Command;
using ByReplace.Printers;

var builder = CoconaApp.CreateBuilder(
    new[]
    {
        "apply",
        "rules",
        @"-p C:\Users\iel_1\Documents\TestLieu",
        @"-f C:\Users\iel_1\Documents\Projetos\ByReplace\src\ByReplace"
    });
builder.Services.AddScoped<IPrint, PrintConsole>();
var app = builder.Build();

app
    .AddSubCommand("apply", apply =>
    {
        apply.AddCommand("rule", async (ApplyRuleParameters applyRuleParameters) =>
        {
            CancellationTokenSource source = new CancellationTokenSource();
            CancellationToken token = source.Token;

            var configuration = BrConfigurationBuilder
                .Instantiate()
                .SetRule(applyRuleParameters.Rule)
                .SetPath(applyRuleParameters.Path)
                .SetConfigPath(applyRuleParameters.ConfigFile)
                .Build();

            CompositeCommand compositeCommand = new CompositeCommand(new ICommand[]
            {
                new PrintLogoCommand(),
                new PrintBRVersionCommand(),
                new ApplyRuleCommand(configuration, applyRuleParameters)
            });

            await compositeCommand.ExecuteAsync(token);
        });

        apply.AddCommand("rules", async (ApplyParameters applyParameters) =>
        {
            CancellationTokenSource source = new CancellationTokenSource();
            CancellationToken token = source.Token;

            var configuration = BrConfigurationBuilder
               .Instantiate()
               .SetPath(applyParameters.Path)
               .SetConfigPath(applyParameters.ConfigFile)
               .Build();

            CompositeCommand compositeCommand = new CompositeCommand(new ICommand[]
            {
                new PrintLogoCommand(),
                new PrintBRVersionCommand(),
                new ApplyRulesCommand(configuration, applyParameters)
            });

            await compositeCommand.ExecuteAsync(token);
        });

        apply.AddCommand("list-rules", () =>
        {
            //Print all rule's names from config file
        });

        apply.AddCommand("open-rule", () =>
        {
            //Print rule in config file
        });
    })
    .WithDescription("apply commands");

app.AddCommand("verify", () => { });
app.AddCommand("take", () => { });

app.Run();