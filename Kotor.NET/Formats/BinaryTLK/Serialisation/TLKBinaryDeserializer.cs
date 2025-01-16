using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Kotor.NET.Exceptions;
using Kotor.NET.Resources.KotorTLK;

namespace Kotor.NET.Formats.BinaryTLK.Serialisation;

public class TLKBinaryDeserializer
{
    private TLKBinary _binary { get; }

    public TLKBinaryDeserializer(TLKBinary binary)
    {
        _binary = binary;
    }

    public TLK Deserialize()
    {
        try
        {
            var tlk = new TLK();

            for (int i = 0; i < _binary.Entries.Count; i++)
            {
                var text = _binary.Strings[i];
                var sound = _binary.Entries[i].SoundResRef;
                tlk.Add(text, sound);
            }

            return tlk;
        }
        catch (Exception e)
        {
            throw new DeserializationException("Failed to deserialize the TLK data.", e);
        }
    }
}
