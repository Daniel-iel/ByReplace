namespace ByReplace.Test.Common.ConfigMock;

public class ContentSyntax
{
    public ContentSyntax()
    {

    }

    private ContentSyntax(string path, List<string> skipDirectories)
    {
        Path = path;
        SkipDirectories = skipDirectories;
    }

    public string Path { get; set; }
    public List<string> SkipDirectories { get; set; }
    public List<RuleSyntax> Rules { get; set; }

    public static ContentSyntax Create(string path, List<string> skipDirectories)
    {
        return new ContentSyntax(path, skipDirectories);
    }

    public void AddRule(RuleSyntax ruleSyntax)
    {
        Rules.Add(ruleSyntax);
    }

    public void AddRules(List<RuleSyntax> rulesSyntax)
    {
        Rules.AddRange(rulesSyntax);
    }
}
