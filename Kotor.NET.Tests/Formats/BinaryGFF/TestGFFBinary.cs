using System.Reflection.PortableExecutable;
using Kotor.NET.Common;
using Kotor.NET.Common.Data;
using Kotor.NET.Extensions;
using Kotor.NET.Formats.BinaryGFF;
using Kotor.NET.Resources.KotorGFF;
using Xunit;

namespace Kotor.NET.Tests.Formats.BinaryGFF;

public class TestGFFBinary
{
    public static readonly string File1Filepath = "Formats/BinaryGFF/file1.gff";

    private GFFBinary GetBinaryGFF(byte[] data)
    {
        return new GFFBinary(new MemoryStream(data));
    }

    [Fact]
    public void Test_RecalculateFile1()
    {
        var binaryGFF = GetBinaryGFF(File.ReadAllBytes(File1Filepath));
        binaryGFF.FileHeader.StructCount = Int32.MinValue;
        binaryGFF.FileHeader.FieldCount = Int32.MinValue;
        binaryGFF.FileHeader.LabelCount = Int32.MinValue;
        binaryGFF.FileHeader.OffsetToStructs = Int32.MinValue;
        binaryGFF.FileHeader.OffsetToFields = Int32.MinValue;
        binaryGFF.FileHeader.OffsetToLabels = Int32.MinValue;
        binaryGFF.FileHeader.OffsetToFieldData = Int32.MinValue;
        binaryGFF.FileHeader.OffsetToFieldIndices = Int32.MinValue;
        binaryGFF.FileHeader.OffsetToListIndicies = Int32.MinValue;
        binaryGFF.Recalculate();

        var stream = new MemoryStream();
        var reader = new BinaryReader(stream);
        binaryGFF.Write(stream);


        stream.Position = 0;
        var fileHeader = new GFFBinaryFileHeader(reader);
        Assert.Equal(3, binaryGFF.FileHeader.StructCount);
        Assert.Equal(4, binaryGFF.FileHeader.FieldCount);
        Assert.Equal(4, binaryGFF.FileHeader.LabelCount);

        stream.Position = binaryGFF.FileHeader.OffsetToStructs;
        var struct0 = new GFFBinaryStruct(reader);
        Assert.Equal(UInt32.MaxValue, struct0.ID);
        Assert.Equal<uint>(3, struct0.FieldCount);
        Assert.Equal(0, struct0.DataOrDataOffset);

        stream.Position = binaryGFF.FileHeader.OffsetToFields;
        var field0 = new GFFBinaryField(reader);
        Assert.Equal((int)GFFBinaryFieldType.String, field0.Type);
        Assert.Equal<uint>(0, field0.LabelIndex);

        stream.Position = binaryGFF.FileHeader.OffsetToLabels;
        var label0 = reader.ReadString(16);
        Assert.Equal("Field0", label0);
    }
}
