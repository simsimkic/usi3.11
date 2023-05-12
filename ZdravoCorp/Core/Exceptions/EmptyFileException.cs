using System;
using System.Runtime.Serialization;

namespace ZdravoCorp.Core.Exceptions;

public class EmptyFileException : Exception
{
    protected EmptyFileException(SerializationInfo info, StreamingContext context) : base(info, context)
    {
    }

    public EmptyFileException(string? message) : base(message)
    {
    }

    public EmptyFileException(string? message, Exception? innerException) : base(message, innerException)
    {
    }
}