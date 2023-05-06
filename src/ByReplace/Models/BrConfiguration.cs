namespace ByReplace.Models;

internal class BrConfiguration
{
    public BrConfiguration(string path, string[] skipDirectories, ImmutableList<Rule> rules)
    {
        this.Path = path;
        this.SkipDirectories = skipDirectories;
        this.Rules = rules;
    }

    public string Path { get; private set; }
    public string[] SkipDirectories { get; }
    public ImmutableList<Rule> Rules { get; private set; }

    public void SetOnlyOneRule(Rule rule)
    {
        Rules = ImmutableList.Create(rule);
    }

    public void ChangeDefaultPath(string path)
    {
        string sanitizedPath = Sanitizer(path);

        if (!Directory.Exists(sanitizedPath))
        {
            throw new DirectoryNotFoundException($"Path {sanitizedPath} does not exists.");
        }

        Path = Sanitizer(sanitizedPath);
    }

    public static BrConfiguration GetConfiguration(string pathConfig)
    {
        string configurationFilePath = Sanitizer(string.Join(@"\", pathConfig, "brconfig.json"));

#pragma warning disable IDE0046 // Convert to conditional expression
        if (!File.Exists(configurationFilePath))
        {
            throw new FileNotFoundException($"BR Configuration not found on {pathConfig} path.");
        }
#pragma warning restore IDE0046 // Convert to conditional expression

        return JsonSerializer.Deserialize<BrConfiguration>(File.ReadAllText(configurationFilePath, Encoding.UTF8));
    }

    private static string Sanitizer(string param)
    {
        return param.Trim();
    }
}
