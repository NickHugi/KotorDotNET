using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Kotor.NET.Common.Data;
using Kotor.NET.Exceptions;
using Kotor.NET.Resources.KotorRIM;

namespace Kotor.NET.Formats.BinaryRIM.Serialisation;

public class RIMBinaryDeserializer
{
    private RIMBinary _binary { get; }

    public RIMBinaryDeserializer(RIMBinary binary)
    {
        _binary = binary;
    }

    public RIM Deserialize()
    {
        try
        {
            var rim = new RIM();

            for (int i = 0; i < _binary.ResourceEntries.Count; i++)
            {
                var resref = _binary.ResourceEntries[i].ResRef;
                var type = ResourceType.ByID(_binary.ResourceEntries[i].ResourceTypeID);
                var data = _binary.ResourceData[i];
                rim.Add(resref, type, data);
            }

            return rim;
        }
        catch (Exception e)
        {
            throw new DeserializationException("Failed to deserialize the RIM data.", e);
        }
    }
}
