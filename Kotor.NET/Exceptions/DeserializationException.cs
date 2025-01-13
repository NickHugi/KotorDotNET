using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kotor.NET.Exceptions;

public class DeserializationException : Exception
{
    public DeserializationException() : base("An error occurred during the Deserialization process.")
    {
    }

    public DeserializationException(string message) : base(message)
    {
    }

    public DeserializationException(string message, Exception innerException) : base(message, innerException)
    {
    }
}
