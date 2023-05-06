namespace ByReplace.Exceptions;

internal class RuleNotFoundException : Exception
{
    public RuleNotFoundException() { }

    public RuleNotFoundException(string? message) : base(message)
    {
    }

    public RuleNotFoundException(string? message, Exception? innerException) : base(message, innerException)
    {
    }

    public static void ThrowIfNull([NotNull] object? argument, [CallerArgumentExpression("argument")] string? paramName = null)
    {
        if (argument is null)
        {
            Throw(paramName);
        }
    }

    [DoesNotReturn]
    private static void Throw(string? paramName)
        => throw new RuleNotFoundException(paramName);
}
