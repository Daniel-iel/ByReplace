namespace ByReplace.Models
{
    internal record Rule(string Name, string Description, string[] Skip, string[] Extensions, Replacement Replacement);
}
