namespace ByReplace.Test.Common.ConfigMock;

internal sealed class BrContentFactory
{
    private ContentSyntax _configSyntax;
    private List<RuleSyntax> _rulesSyntax = new List<RuleSyntax>();

    public static BrContentFactory CreateDefault() => new BrContentFactory();

    public BrContentFactory AddConfig(ContentSyntax configSyntax)
    {
        _configSyntax = configSyntax;

        return this;
    }

    public BrContentFactory AddRule(RuleSyntax ruleSyntax)
    {
        _rulesSyntax.Add(ruleSyntax);
        return this;
    }

    public BrContentFactory AddRules(params RuleSyntax[] rulesSyntax)
    {
        _rulesSyntax.AddRange(rulesSyntax);
        return this;
    }

    public static ContentSyntax ConfigDeclaration(string path, params string[] skipDirectories)
    {
        return ContentSyntax.Create(path, [.. skipDirectories]);
    }
    public static ContentSyntax ConfigNoPathDeclaration(params string[] skipDirectories)
    {
        return ContentSyntax.Create("", [.. skipDirectories]);
    }

    public static RuleSyntax Rule(string name, string description)
    {
        return RuleSyntax.Create(name, description);
    }
    public static RuleSyntax Rule(string name)
    {
        return RuleSyntax.Create(name);
    }

    public static ReplacementSyntax Replacement(string @new, params string[] Old)
    {
        return ReplacementSyntax.Create(@new, [.. Old]);
    }

    public ContentSyntax Compile()
    {
        _configSyntax.Rules = _rulesSyntax;

        return _configSyntax;
    }
}
