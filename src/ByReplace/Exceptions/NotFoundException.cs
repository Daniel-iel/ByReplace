﻿namespace ByReplace.Exceptions;

[ExcludeFromCodeCoverage]
internal abstract class NotFoundException : Exception
{
    protected NotFoundException() { }

    protected NotFoundException(string message) : base(message)
    {
    }

    protected NotFoundException(string message, Exception innerException) : base(message, innerException)
    {
    }
}
