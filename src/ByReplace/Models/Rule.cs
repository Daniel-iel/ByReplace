
[assembly: InternalsVisibleTo("ByReplace.Test")]

namespace ByReplace.Models;

internal record class Rule(
    string Name,
    string Description,
    string[] Skip,
    string[] Extensions,
    Replacement Replacement);
