[assembly: InternalsVisibleTo("ByReplace.Test")]

namespace ByReplace.Commands.Apply.Rule;

internal record class ApplyRuleParameter : ApplyParameter
{
    [Option(shortName: 'r', Description = "Rule name to be applied.")]
    public string Rule { get; set; }
}
