namespace ByReplace.Test.Common.ConfigMock;

public class ReplacementSyntax
{
    public ReplacementSyntax(string @new, List<string> Old)
    {
        this.Old = Old;
        New = @new;
    }

    public List<string> Old { get; set; }
    public string New { get; set; }

    public static ReplacementSyntax Create(string @new, List<string> Old)
    {
        return new ReplacementSyntax(@new, Old);
    }
}
