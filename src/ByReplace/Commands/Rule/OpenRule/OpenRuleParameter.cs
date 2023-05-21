namespace ByReplace.Commands.Rule.OpenRule;

internal class OpenRuleParameter : ICommandParameterSet
{
    [Option(shortName: 'n', Description = "Rule's name")]
    public string Name { get; set; }

    [Option(shortName: 'f', Description = "Path of the brconfig file.")]
    public string ConfigFile { get; set; }
}
