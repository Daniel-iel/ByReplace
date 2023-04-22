using ByReplace.Builders;
using ByReplace.Commands.Apply.Parameters;
using ByReplace.Commands.Apply.Rule;
using ByReplace.Commands.Handlers;
using ByReplace.Commands.Logo;
using Cocona;
//var app = CoconaApp.Create(
//    new[]
//    {
//        "apply",
//        "rule",
//        "-r RemoveServiceBus",
//        @"-p C:\Users\iel_1\Documents\TestLieu",
//        @"-f C:\Users\iel_1\Documents\Projetos\ByReplace\src\ByReplace"
//    });

var builder = CoconaApp.CreateBuilder();
var app = builder.Build();

app.AddSubCommand("apply", apply =>
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