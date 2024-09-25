using Xunit;

namespace ByReplace.Test.TestHelpers.Attributes;
public enum Platform
{
    Windows,
    Linux
}

public class PlatformSpecificFactAttribute : FactAttribute
{
    public PlatformSpecificFactAttribute(Platform platform)
    {
        if ((platform == Platform.Windows && !OperatingSystem.IsWindows()) ||
            (platform == Platform.Linux && !OperatingSystem.IsLinux()))
        {
            Skip = $"Test is only valid on {platform}.";
        }
    }
}
