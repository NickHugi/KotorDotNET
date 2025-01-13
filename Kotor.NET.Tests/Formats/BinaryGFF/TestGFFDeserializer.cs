using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Kotor.NET.Common.Data;
using Kotor.NET.Formats.Binary2DA.Serialisation;
using Kotor.NET.Formats.BinaryGFF;

namespace Kotor.NET.Tests.Formats.BinaryGFF;

public class TestGFFDeserializer
{
    public static readonly string File1Filepath = "Formats/BinaryGFF/file1.gff";


    [Fact]
    public void Deserialize()
    {
        using var stream = File.OpenRead(File1Filepath);
        var binaryGFF = new GFFBinary(stream);
        var gff = new GFFBinaryDeserializer(binaryGFF).Deserialize();

        Assert.Equal(3, gff.Root.FieldCount());
        Assert.Equal("text", gff.Root.GetString("Field0"));
        Assert.Equal<uint?>(5, gff.Root.GetList("List0")?.ElementAt(0).ID);
        Assert.Equal<byte?>(123, gff.Root.GetStruct("Struct0")?.GetUInt8("Field1"));
    }
}
