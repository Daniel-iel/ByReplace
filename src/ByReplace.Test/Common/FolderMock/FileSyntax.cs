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

    public string Name { get; private set; }
    public string Content { get; private set; }

    public string Extension => System.IO.Path.GetExtension(Name);

    public static FileSyntax New()
    {
        return new FileSyntax("");
    }

    public static FileSyntax FileDeclaration(string name)
    {
        return new FileSyntax(name);
    }

    public static FileSyntax FileDeclaration(string name, string content)
    {
        return new FileSyntax(name, content);
    }

    public FileSyntax Create(string name, string content)
    {
        return new FileSyntax(name, content);
    }

    public FileSyntax AddName(string name)
    {
        Name = name;
        return this;
    }

    public FileSyntax AddContent(string content)
    {
        Content = content;
        return this;
    }
}
