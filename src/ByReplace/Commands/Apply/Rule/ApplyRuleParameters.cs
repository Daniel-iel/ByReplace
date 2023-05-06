namespace ByReplace.Commands.Apply.Rule;

internal record ApplyRuleParameters : ApplyParameters
{
    [Option(shortName: 'r', Description = "Rule name to be applied.")]
    public string Rule { get; set; }
}
