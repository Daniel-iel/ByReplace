namespace ByReplace.Test.Common.FolderMock;

internal sealed class FileSyntax
{
    public FileSyntax(string name)
    {
        Name = name;
    }

    public FileSyntax(string name, string content)
    {
        Name = name;
        Content = content;
    }

    public string Name { get; }
    public string Content { get; }

    public string Extension => System.IO.Path.GetExtension(Name);

    public static FileSyntax FileDeclaration(string name)
    {
        return new FileSyntax(name);
    }

    public static FileSyntax FileDeclaration(string name, string content)
    {
        return new FileSyntax(name, content);
    }
}
