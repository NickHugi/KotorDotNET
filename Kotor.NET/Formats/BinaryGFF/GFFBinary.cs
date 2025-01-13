using System;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using Kotor.NET.Common;
using Kotor.NET.Common.Data;
using Kotor.NET.Extensions;
using Kotor.NET.Helpers;
using Kotor.NET.Resources.KotorGFF;

namespace Kotor.NET.Formats.BinaryGFF;

public class GFFBinary
{
    public GFFBinaryFileHeader FileHeader { get; set; } = new();
    public List<GFFBinaryStruct> Structs { get; set; } = new();
    public List<GFFBinaryField> Fields { get; set; } = new();
    public List<string> Labels { get; set; } = new();
    public byte[] FieldData { get; set; } = new byte[0];
    public byte[] FieldIndices { get; set; } = new byte[0];
    public byte[] ListIndices { get; set; } = new byte[0];

    public GFFBinary()
    {
    }
    public GFFBinary(Stream stream)
    {
        var reader = new BinaryReader(stream);

        FileHeader = new GFFBinaryFileHeader(reader);

        reader.BaseStream.Position = FileHeader.OffsetToStructs;
        for (int i = 0; i < FileHeader.StructCount; i++)
        {
            Structs.Add(new GFFBinaryStruct(reader));
        }

        reader.BaseStream.Position = FileHeader.OffsetToFields;
        for (int i = 0; i < FileHeader.FieldCount; i++)
        {
            Fields.Add(new GFFBinaryField(reader));
        }

        reader.BaseStream.Position = FileHeader.OffsetToLabels;
        for (int i = 0; i < FileHeader.LabelCount; i++)
        {
            Labels.Add(reader.ReadString(16));
        }

        reader.BaseStream.Position = FileHeader.OffsetToFieldData;
        FieldData = reader.ReadBytes((int)FileHeader.FieldDataLength);

        reader.BaseStream.Position = FileHeader.OffsetToFieldIndices;
        FieldIndices = reader.ReadBytes((int)FileHeader.FieldIndiciesLength);

        reader.BaseStream.Position = FileHeader.OffsetToListIndicies;
        ListIndices = reader.ReadBytes((int)FileHeader.ListIndiciesLength);
    }

    public void Write(Stream stream)
    {
        var writer = new BinaryWriter(stream);

        FileHeader.Write(writer);

        writer.BaseStream.Position = FileHeader.OffsetToStructs;
        foreach (var @struct in Structs)
        {
            @struct.Write(writer);
        }

        writer.BaseStream.Position = FileHeader.OffsetToFields;
        foreach (var field in Fields)
        {
            field.Write(writer);
        }

        writer.BaseStream.Position = FileHeader.OffsetToLabels;
        foreach (var label in Labels)
        {
            writer.Write(label.Truncate(16).PadRight(16, '\0'), 0);
        }

        writer.BaseStream.Position = FileHeader.OffsetToFieldData;
        writer.Write(FieldData);

        writer.BaseStream.Position = FileHeader.OffsetToFieldIndices;
        writer.Write(FieldIndices);

        writer.BaseStream.Position = FileHeader.OffsetToListIndicies;
        writer.Write(ListIndices);
    }

    public void Recalculate()
    {
        FileHeader.FieldCount = Fields.Count;
        FileHeader.StructCount = Structs.Count;
        FileHeader.LabelCount = Labels.Count;
        FileHeader.FieldDataLength = FieldData.Count();
        FileHeader.FieldIndiciesLength = FieldIndices.Count();
        FileHeader.ListIndiciesLength = ListIndices.Count();

        var offset = GFFBinaryFileHeader.SIZE;
        FileHeader.OffsetToStructs = offset;

        offset += GFFBinaryStruct.SIZE * Structs.Count;
        FileHeader.OffsetToFields = offset;

        offset += GFFBinaryField.SIZE * Fields.Count;
        FileHeader.OffsetToLabels = offset;

        offset += 16 * Labels.Count;
        FileHeader.OffsetToFieldData = offset;

        offset += FieldData.Length;
        FileHeader.OffsetToFieldIndices = offset;

        offset += FieldIndices.Length;
        FileHeader.OffsetToListIndicies = offset;
    }
}
