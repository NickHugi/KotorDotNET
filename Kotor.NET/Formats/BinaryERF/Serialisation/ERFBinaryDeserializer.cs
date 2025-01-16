using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Kotor.NET.Common.Data;
using Kotor.NET.Exceptions;
using Kotor.NET.Resources.KotorERF;

namespace Kotor.NET.Formats.BinaryERF.Serialisation;

public class ERFBinaryDeserializer
{
    private ERFBinary _binary { get; }

    public ERFBinaryDeserializer(ERFBinary binary)
    {
        _binary = binary;
    }

    public ERF Deserialize()
    {
        try
        {
            var erf = new ERF();

            for (int i = 0; i < _binary.ResourceEntries.Count; i++)
            {
                var resref = _binary.KeyEntries[i].ResRef;
                var type = ResourceType.ByID(_binary.KeyEntries[i].ResType);
                var data = _binary.ResourceData[i];
                erf.Add(resref, type, data);
            }

            return erf;
        }
        catch (Exception e)
        {
            throw new DeserializationException("Failed to deserialize the ERF data.", e);
        }
    }
}
