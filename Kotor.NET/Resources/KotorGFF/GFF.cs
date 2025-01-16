using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Kotor.NET.Formats.Binary2DA.Serialisation;
using Kotor.NET.Formats.BinaryGFF;

namespace Kotor.NET.Resources.KotorGFF;

public class GFF
{
    public static GFF FromFile(string filepath)
    {
        using var stream = File.OpenRead(filepath);
        return FromStream(stream);
    }
    public static GFF FromBytes(byte[] bytes)
    {
        using var stream = new MemoryStream(bytes);
        return FromStream(stream);
    }
    public static GFF FromStream(Stream stream)
    {
        var binary = new GFFBinary(stream);
        var deserializer = new GFFBinaryDeserializer(binary);
        return deserializer.Deserialize();
    }

    public GFFType Type { get; set; }
    public GFFStruct Root { get; set; } = new();
}
