namespace ByReplace.Exceptions;

internal abstract class NotfoundException : Exception
{
    protected NotfoundException() { }

    protected NotfoundException(string? message) : base(message)
    {
    }

    protected NotfoundException(string? message, Exception? innerException) : base(message, innerException)
    {
    }
}
