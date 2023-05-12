using System;
using System.Runtime.Serialization;

namespace ZdravoCorp.Core.Exceptions;

public class EmptyListException : Exception
{
    public EmptyListException()
    {
    }

    protected EmptyListException(SerializationInfo info, StreamingContext context) : base(info, context)
    {
    }

    public EmptyListException(string? message) : base(message)
    {
    }

    public EmptyListException(string? message, Exception? innerException) : base(message, innerException)
    {
    }
}