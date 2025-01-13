using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Kotor.NET.Common;
using Kotor.NET.Common.Data;
using Kotor.NET.Formats.Binary2DA.Serialisation;
using Kotor.NET.Formats.BinaryERF;
using Kotor.NET.Formats.BinaryERF.Serialisation;
using Kotor.NET.Formats.BinaryGFF;
using Kotor.NET.Resources.KotorERF;
using Kotor.NET.Resources.KotorGFF;

namespace Kotor.NET.Tests.Formats.BinaryGFF;

public class TestGFFSerializer
{
    public static readonly string File1Filepath = "Formats/BinaryGFF/file1.gff";

    [Fact]
    public void Serialize()
    {
        var gff = new GFF();
        gff.Root.SetInt32("Int32", 123);
        gff.Root.SetInt64("Int64", (long)Int32.MaxValue + 1);
        var struct1 = gff.Root.SetStruct("Struct", 1);
        struct1.SetInt8("Int8", 12);
        var list = gff.Root.SetList("List");
        var struct2 = list.Add(2);
        var struct3 = list.Add(3);
        var struct4 = struct3.SetStruct("Struct", 4);
        struct4.SetResRef("ResRef", "abc");
        var struct5 = list.Add(5);
        struct5.SetString("String", "def");
        struct5.SetLocalisedString("LocalisedString1", new(13));
        struct5.SetLocalisedString("LocalisedString2", new(new List<LocalisedSubstring> { new(Language.German, Gender.Female, "Guten Tag") }));
        struct5.SetVector3("Vector3", new(1, 2, 3));
        struct5.SetVector4("Vector4", new(1, 2, 3, 4));
        struct5.SetBinary("Binary", new byte[] { 1, 2, 3 });

        var binaryGFF = new GFFBinarySerializer(gff).Serialize();
        var reloadGFF = new GFFBinaryDeserializer(binaryGFF).Deserialize();

        Assert.Equal(reloadGFF.Root.GetInt32("Int32"), 123);
        Assert.Equal(reloadGFF.Root.GetInt64("Int64"), (long)Int32.MaxValue + 1);
        Assert.Equal<uint?>(reloadGFF.Root.GetStruct("Struct")?.ID, 1);
        Assert.Equal<sbyte?>(12, reloadGFF.Root.GetStruct("Struct")?.GetInt8("Int8"));
        Assert.Equal<uint?>(reloadGFF.Root.GetList("List")?.ElementAt(0)?.ID, 2);
        Assert.Equal<uint?>(reloadGFF.Root.GetList("List")?.ElementAt(1)?.ID, 3);
        Assert.Equal<uint?>(reloadGFF.Root.GetList("List")?.ElementAt(1)?.GetStruct("Struct")?.ID, 4);
        Assert.Equal("abc", reloadGFF.Root.GetList("List")?.ElementAt(1)?.GetStruct("Struct")?.GetResRef("ResRef")?.Get());
        Assert.Equal("def", reloadGFF.Root.GetList("List")?.ElementAt(2)?.GetString("String"));
        Assert.Equal(reloadGFF.Root.GetList("List")?.ElementAt(2)?.GetLocalisedString("LocalisedString1")?.StringRef, 13);
        Assert.Equal("Guten Tag", reloadGFF.Root.GetList("List")?.ElementAt(2)?.GetLocalisedString("LocalisedString2")?.GetSubstring(Language.German, Gender.Female));
        Assert.Equal(reloadGFF.Root.GetList("List")?.ElementAt(2)?.GetBinary("Binary"), new byte[] { 1, 2, 3 });
    }
}
