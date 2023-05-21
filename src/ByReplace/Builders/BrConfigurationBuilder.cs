namespace ByReplace.Builders;

internal class BrConfigurationBuilder
{
    private string _path;
    private string _configFile;
    private string _rule;

    internal static BrConfigurationBuilder Instantiate()
    {
        return new BrConfigurationBuilder();
    }

    internal BrConfigurationBuilder SetConfigPath(string configFile)
    {
        ArgumentNullException.ThrowIfNull(configFile);

        _configFile = configFile;

        return this;
    }

    internal BrConfigurationBuilder SetPath(string path)
    {
        ArgumentNullException.ThrowIfNull(path);

        _path = path;

        return this;
    }

    internal BrConfigurationBuilder SetRule(string rule)
    {
        ArgumentNullException.ThrowIfNull(rule);

        _rule = Sanitizer(rule);

        return this;
    }

    public BrConfiguration Build()
    {
        var configuration = BrConfiguration.GetConfiguration(_configFile);

        if (!string.IsNullOrEmpty(_rule))
        {
            var rule = configuration.Rules
                .Where(r => r.Name.Equals(_rule, StringComparison.InvariantCultureIgnoreCase))
                .FirstOrDefault();

            //RuleNotFoundException.ThrowIfNull(rule, $"Rule {_rule} was not found in brconfig file.");

            // dado um arquivo com várias regras, esse código vai fazer o usuário escolher apenas uma
            // regra para ser aplicada individualmente.
            configuration.SetOnlyOneRule(rule);
        }

        if (!string.IsNullOrEmpty(_path))
        {
            configuration.ChangeDefaultPath(_path);
        }

        return configuration;
    }

    private static string Sanitizer(string param)
    {
        return param.Trim();
    }
}
