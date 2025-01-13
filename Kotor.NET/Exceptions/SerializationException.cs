using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kotor.NET.Exceptions;

public class SerializationException : Exception
{
    public SerializationException() : base("An error occurred during the serialization process.")
    {
    }

    public SerializationException(string message) : base(message)
    {
    }

    public SerializationException(string message, Exception innerException) : base(message, innerException)
    {
    }
}
