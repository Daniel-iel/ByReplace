namespace ByReplace.Test.Common.FolderMock;

internal sealed class FileSyntaxV2
{
    public FileSyntaxV2(string name)
    {
        Name = name;
    }

    public FileSyntaxV2(string name, string content)
    {
        Name = name;
        Content = content;
    }

    public string Name { get; private set; }
    public string Content { get; private set; }

    public string Extension => System.IO.Path.GetExtension(Name);

    public static FileSyntaxV2 New()
    {
        return new FileSyntaxV2("");
    }

    public static FileSyntaxV2 FileDeclaration(string name)
    {
        return new FileSyntaxV2(name);
    }

    public static FileSyntaxV2 FileDeclaration(string name, string content)
    {
        return new FileSyntaxV2(name, content);
    }

    public FileSyntaxV2 Create(string name, string content)
    {
        return new FileSyntaxV2(name, content);
    }

    public FileSyntaxV2 AddName(string name)
    {
        Name = name;
        return this;
    }

    public FileSyntaxV2 AddContent(string content)
    {
        Content = content;
        return this;
    }
}
