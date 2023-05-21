namespace ByReplace.Commands.Rule.ListRules
{
    internal class ListRulesParameter : ICommandParameterSet
    {
        [Option(shortName: 'f', Description = "Path of the brconfig file.")]
        public string ConfigFile { get; set; }
    }
}
