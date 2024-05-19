namespace ByReplace.Commands.Rule.ListRules;

internal sealed class ListRulesParameter : ICommandParameterSet
{
    [Option(shortName: 'f', Description = "Path of the brconfig file.")]
    public string ConfigFile { get; set; }
}
