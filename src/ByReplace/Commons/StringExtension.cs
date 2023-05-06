namespace ByReplace.Commons;

public static class StringExtension
{
    public static bool Contains(this string @value, params string[] values)
    {
        foreach (var stringContains in values)
        {
            if (@value.Equals(stringContains, StringComparison.InvariantCultureIgnoreCase))
            {
                return true;
            }
        }

        return false;
    }
}
