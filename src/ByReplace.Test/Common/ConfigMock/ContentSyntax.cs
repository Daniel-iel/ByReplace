namespace ByReplace.Test.Common.ConfigMock;

public class ContentSyntax
{
    public ContentSyntax()
    {
        SkipDirectories = new List<string>();
        Rules = new List<RuleSyntax>();
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

    public ContentSyntax AddPath(string path)
    {
        Path = path;

        return this;
    }

    public ContentSyntax AddSkip(params string[] skipDirectories)
    {
        SkipDirectories = [.. skipDirectories];

        return this;
    }

    public void AddRule(RuleSyntax ruleSyntax)
    {
        Rules.Add(ruleSyntax);
    }

    public void AddRules(List<RuleSyntax> rulesSyntax)
    {
        Rules.AddRange(rulesSyntax);
    }

    public ContentSyntax AddRules(params Action<RuleSyntax>[] actions)
    {

        foreach (var action in actions)
        {
            var ruleSyntax = new RuleSyntax();
            action(ruleSyntax);
            Rules.Add(ruleSyntax);
        }

        return this;
    }
}
