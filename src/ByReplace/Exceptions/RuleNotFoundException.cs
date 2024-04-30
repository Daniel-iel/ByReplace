namespace ByReplace.Exceptions;

[ExcludeFromCodeCoverage]
internal class RuleNotFoundException : NotFoundException
{
    public RuleNotFoundException() : base() { }

    public RuleNotFoundException(string message) : base(message)
    {
    }

    public RuleNotFoundException(string message, Exception innerException) : base(message, innerException)
    {
    }

    public static void ThrowIfNull([NotNull] object argument, [CallerArgumentExpression("argument")] string paramName = null)
    {
        if (argument is null)
        {
            Throw(paramName);
        }
    }

    [DoesNotReturn]
    private static void Throw(string paramName)
        => throw new RuleNotFoundException(paramName);
}
