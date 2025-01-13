using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Kotor.NET.Exceptions;
using Kotor.NET.Resources.KotorLIP;

namespace Kotor.NET.Formats.BinaryLIP.Serialisation;

public class LIPBinaryDeserializer
{
    private LIPBinary _binary { get; }

    public LIPBinaryDeserializer(LIPBinary binary)
    {
        _binary = binary;
    }

    public LIP Deserialize()
    {
        try
        {
            var lip = new LIP();

            _binary.KeyFrames.ForEach(frame =>
            {
                lip.Add(frame.Time, (LIPMouthShape)frame.Shape);
            });

            return lip;
        }
        catch (Exception e)
        {
            throw new DeserializationException("Failed to deserialize the LIP data.", e);
        }
    }
}
