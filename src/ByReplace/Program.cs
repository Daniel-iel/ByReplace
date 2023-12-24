using ByReplace.Builders;
using ByReplace.Commands.Apply.Rules;
using ByReplace.Commands.Rule.ListRules;
using ByReplace.Commands.Rule.OpenRule;
using ByReplace.Commands.TimerFinish;
using Cocona.Builder;

#if DEBUG
    CoconaAppBuilder builder = CoconaApp.CreateBuilder(
        new[]
        {
            "apply",
            "rules",
            @"-p C:\Projetos\Daniel-iel\ByReplace\samples",
            @"-f C:\Projetos\Daniel-iel\ByReplace\src\ByReplace"
        });
#else
    CoconaAppBuilder builder = CoconaApp.CreateBuilder();
#endif

builder.Services.AddScoped<IPrint, PrintConsole>();
CoconaApp app = builder.Build();

app.UseFilter(new GlobalHandleExceptionAttribute());

#region .: Apply :.
app
    .AddSubCommand("apply", apply =>
    {
        apply.AddCommand("rule", async (ApplyRuleParameter applyRuleParameters, IPrint print) =>
        {
            CancellationTokenSource source = new CancellationTokenSource();
            CancellationToken token = source.Token;

            BrConfiguration configuration = BrConfigurationBuilder
                .Create()
                .SetRule(applyRuleParameters.Rule)
                .SetPath(applyRuleParameters.Path)
                .SetConfigPath(applyRuleParameters.ConfigFile)
                .Build();

            CompositeCommand compositeCommand = new CompositeCommand(new ICommand[]
            {
                new PrintLogoCommand(),
                new PrintBRVersionCommand(),
                new ApplyRuleCommand(configuration, applyRuleParameters, print),
                new TimerFinishCommand(print)
            });

            await compositeCommand.ExecuteAsync(token);
        });

        apply.AddCommand("rules", async (ApplyParameter applyParameters, IPrint print) =>
        {
            CancellationTokenSource source = new CancellationTokenSource();
            CancellationToken token = source.Token;

            BrConfiguration configuration = BrConfigurationBuilder
               .Create()
               .SetPath(applyParameters.Path)
               .SetConfigPath(applyParameters.ConfigFile)
               .Build();

            CompositeCommand compositeCommand = new CompositeCommand(new ICommand[]
            {
                new PrintLogoCommand(),
                new PrintBRVersionCommand(),
                new ApplyRulesCommand(configuration, print),
                new TimerFinishCommand(print)
            });

            await compositeCommand.ExecuteAsync(token);
        });
    })
    .WithDescription("apply commands");
#endregion

#region .: Rule :.
app
    .AddSubCommand("rule", rule =>
    {
        rule.AddCommand("list-rules", async (ListRulesParameter listRulesParameter, IPrint print) =>
        {
            //Print all rule's names from config file
            CancellationTokenSource source = new CancellationTokenSource();
            CancellationToken token = source.Token;

            BrConfiguration configuration = BrConfigurationBuilder
             .Create()
             .SetConfigPath(listRulesParameter.ConfigFile)
             .Build();

            CompositeCommand compositeCommand = new CompositeCommand(new ICommand[]
            {
                new PrintLogoCommand(),
                new PrintBRVersionCommand(),
                new ListRulesCommand(configuration,print),
                new TimerFinishCommand(print)
            });

            await compositeCommand.ExecuteAsync(token);
        });

        rule.AddCommand("open-rule", async (OpenRuleParameter openRuleParameter, IPrint print) =>
        {
            //Print rule in config file
            CancellationTokenSource source = new CancellationTokenSource();
            CancellationToken token = source.Token;

            BrConfiguration configuration = BrConfigurationBuilder
             .Create()
             .SetConfigPath(openRuleParameter.ConfigFile)
             .Build();

            CompositeCommand compositeCommand = new CompositeCommand(new ICommand[]
            {
                new PrintLogoCommand(),
                new PrintBRVersionCommand(),
                new OpenRuleCommand(configuration, openRuleParameter.Name , print),
                new TimerFinishCommand(print)
            });

            await compositeCommand.ExecuteAsync(token);
        });
    })
    .WithDescription("rule commands");
#endregion

await app
    .RunAsync()
    .ConfigureAwait(false);