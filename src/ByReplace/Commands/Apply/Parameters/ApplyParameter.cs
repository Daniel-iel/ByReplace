
namespace ByReplace.Commands.Apply.Parameters;

internal record ApplyParameter : ICommandParameterSet
{
    [Option(shortName: 'p', Description = "Path of the files to be applied to the rule.")]
    public string Path { get; set; }

    [Option(shortName: 'f', Description = "Path of the brconfig file.")]
    public string ConfigFile { get; set; }
}
