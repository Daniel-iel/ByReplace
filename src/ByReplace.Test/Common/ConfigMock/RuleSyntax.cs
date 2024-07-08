namespace ByReplace.Test.Common.ConfigMock;

public sealed class RuleSyntax
{
    public RuleSyntax()
    {

    }

    private RuleSyntax(
        string name,
        string description,
        List<string> skip,
        List<string> extensions,
        ReplacementSyntax replacement)
    {
        Name = name;
        Description = description;
        Skip = skip;
        Extensions = extensions;
        Replacement = replacement;
    }

    private RuleSyntax(string name, string description)
    {
        Name = name;
        Description = description;
    }

    private RuleSyntax(string name)
    {
        Name = name;
    }

    public string Name { get; set; }
    public string Description { get; set; }
    public List<string> Skip { get; set; }
    public List<string> Extensions { get; set; }
    public ReplacementSyntax Replacement { get; set; }

    public static RuleSyntax Create(string name, string description)
    {
        return new RuleSyntax(name, description);
    }

    public static RuleSyntax Create(string name)
    {
        return new RuleSyntax(name);
    }

    public RuleSyntax WithName(string name)
    {
        Name = name;

        return this;
    }

    public RuleSyntax WithReplacement(ReplacementSyntax replacementSyntax)
    {
        Replacement = replacementSyntax;

        return this;
    }

    public RuleSyntax WithSkips(params string[] skip)
    {
        Skip = skip.ToList();

        return this;
    }

    public RuleSyntax WithExtensions(params string[] extensions)
    {
        Extensions = [.. extensions];

        return this;
    }
}
