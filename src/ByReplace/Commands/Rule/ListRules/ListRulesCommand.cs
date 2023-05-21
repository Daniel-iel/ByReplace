namespace ByReplace.Commands.Rule.ListRules
{
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
            StringBuilder rules = new StringBuilder();
            var totalOfRules = configuration.Rules.Count;

            var printer = new PrintBox();
            printer.CreateBox("Rules", 100, totalOfRules * 2);

            foreach (var rule in configuration.Rules)
            {
                rules.AppendLine($"{rule.Name}: {rule.Description}");
            }

            printer.Print(rules.ToString());

            return ValueTask.CompletedTask;
        }
    }
}
