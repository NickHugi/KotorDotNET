using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using Kotor.NET.Common;
using Kotor.NET.Common.Data;
using Kotor.NET.Exceptions;
using Kotor.NET.Formats.BinaryGFF;
using Kotor.NET.Helpers;
using Kotor.NET.Resources.KotorGFF;

namespace Kotor.NET.Formats.Binary2DA.Serialisation;

public class GFFBinaryDeserializer
{
    private GFFBinary _binary { get; }

    public GFFBinaryDeserializer(GFFBinary binary)
    {
        _binary = binary;
    }

    public GFF Deserialize()
    {
        try
        {
            var gff = new GFF();

            gff.Root = ParseStruct(0);

            return gff;
        }
        catch (Exception e)
        {
            throw new DeserializationException("Failed to deserialize the GFF data.", e);
        }
    }

    private GFFStruct ParseStruct(int structIndex)
    {
        var gffStruct = new GFFStruct(_binary.Structs[structIndex].ID);

        if (_binary.Structs[structIndex].FieldCount == 1)
        {
            ParseField(gffStruct, _binary.Structs[structIndex].DataOrDataOffset);
        }
        else if (_binary.Structs[structIndex].FieldCount > 1)
        {
            for (int i = 0; i < _binary.Structs[structIndex].FieldCount; i++)
            {
                var fieldIndex = BitConverter.ToInt32(_binary.FieldIndices, _binary.Structs[structIndex].DataOrDataOffset + (i * 4));
                ParseField(gffStruct, fieldIndex);
            }
        }

        return gffStruct;
    }
    private GFFList ParseList(int fieldIndicesOffset)
    {
        var list = new GFFList();

        var structCount = BitConverter.ToInt32(_binary.ListIndices, fieldIndicesOffset);

        for (int i = 0; i < structCount; i++)
        {
            var structIndex = BitConverter.ToInt32(_binary.ListIndices, fieldIndicesOffset + 4 + (i * 4));
            list.Add(ParseStruct(structIndex));
        }

        return list;
    }
    private void ParseField(GFFStruct parent, int fieldIndex)
    {
        var label = _binary.Labels[(int)_binary.Fields[fieldIndex].LabelIndex];
        var type = (GFFBinaryFieldType)_binary.Fields[fieldIndex].Type;
        var dataOrDataOffset = _binary.Fields[fieldIndex].DataOrDataOffset;
        var dataAsInt32 = BitConverter.ToInt32(_binary.Fields[fieldIndex].DataOrDataOffset);

        if (type == GFFBinaryFieldType.UInt8)
        {
            parent.SetUInt8(label, dataOrDataOffset[0]);
        }
        else if (type == GFFBinaryFieldType.Int8)
        {
            parent.SetInt8(label, unchecked((sbyte)dataOrDataOffset[0]));
        }
        else if (type == GFFBinaryFieldType.UInt16)
        {
            parent.SetUInt16(label, BitConverter.ToUInt16(dataOrDataOffset, 0));
        }
        else if (type == GFFBinaryFieldType.Int16)
        {
            parent.SetInt16(label, BitConverter.ToInt16(dataOrDataOffset, 0));
        }
        else if (type == GFFBinaryFieldType.UInt32)
        {
            parent.SetUInt32(label, BitConverter.ToUInt32(dataOrDataOffset, 0));
        }
        else if (type == GFFBinaryFieldType.Int32)
        {
            parent.SetInt32(label, BitConverter.ToInt32(dataOrDataOffset, 0));
        }
        else if (type == GFFBinaryFieldType.UInt64)
        {
            parent.SetUInt64(label, BitConverter.ToUInt64(_binary.FieldData, dataAsInt32));
        }
        else if (type == GFFBinaryFieldType.Int64)
        {
            parent.SetInt64(label, BitConverter.ToInt64(_binary.FieldData, dataAsInt32));
        }
        else if (type == GFFBinaryFieldType.Single)
        {
            parent.SetSingle(label, BitConverter.ToSingle(dataOrDataOffset, 0));
        }
        else if (type == GFFBinaryFieldType.Double)
        {
            parent.SetDouble(label, BitConverter.ToDouble(_binary.FieldData, dataAsInt32));
        }
        else if (type == GFFBinaryFieldType.String)
        {
            var length = BitConverter.ToInt32(_binary.FieldData, dataAsInt32);
            var text = Encoding.GetEncoding(1252).GetString(_binary.FieldData, dataAsInt32 + 4, length);
            parent.SetString(label, text);
        }
        else if (type == GFFBinaryFieldType.ResRef)
        {
            var length = _binary.FieldData[dataAsInt32];
            var text = Encoding.GetEncoding(1252).GetString(_binary.FieldData, dataAsInt32 + 1, length);
            parent.SetResRef(label, text);
        }
        else if (type == GFFBinaryFieldType.LocalisedString)
        {
            var totalSize = BitConverter.ToInt32(_binary.FieldData, dataAsInt32);
            var stringRef = BitConverter.ToInt32(_binary.FieldData, dataAsInt32 + 4);
            var stringCount = BitConverter.ToInt32(_binary.FieldData, dataAsInt32 + 8);

            var locstring = new LocalisedString();
            locstring.StringRef = stringRef;

            int offset = dataAsInt32 + 12;
            for (int i = 0; i < stringCount; i++)
            {
                var stringID = BitConverter.ToInt32(_binary.FieldData, offset);
                var stringLength = BitConverter.ToInt32(_binary.FieldData, offset + 4);
                var text = KEncoding.GetWindows1252().GetString(_binary.FieldData, offset + 8, stringLength);

                var language = (Language)(stringID >> 1);
                var gender = (Gender)(stringID % 2);
                locstring.SetSubstring(language, gender, text);

                offset += 8 + stringLength;
            }

            parent.SetLocalisedString(label, locstring);
        }
        else if (type == GFFBinaryFieldType.Binary)
        {
            var length = BitConverter.ToInt32(_binary.FieldData, dataAsInt32);
            var data = _binary.FieldData.Skip(dataAsInt32 + 4).Take(length).ToArray();
            parent.SetBinary(label, data);
        }
        else if (type == GFFBinaryFieldType.Struct)
        {
            var child = ParseStruct(dataAsInt32);
            parent.SetStruct(label, child);
        }
        else if (type == GFFBinaryFieldType.List)
        {
            var list = ParseList(dataAsInt32);
            parent.SetList(label, list);
        }
        else if (type == GFFBinaryFieldType.Vector3)
        {
            var x = BitConverter.ToSingle(_binary.FieldData, dataAsInt32);
            var y = BitConverter.ToSingle(_binary.FieldData, dataAsInt32 + 4);
            var z = BitConverter.ToSingle(_binary.FieldData, dataAsInt32 + 8);
            parent.SetVector3(label, new(x, y, z));
        }
        else if (type == GFFBinaryFieldType.Vector3)
        {
            var x = BitConverter.ToSingle(_binary.FieldData, dataAsInt32);
            var y = BitConverter.ToSingle(_binary.FieldData, dataAsInt32 + 4);
            var z = BitConverter.ToSingle(_binary.FieldData, dataAsInt32 + 8);
            var w = BitConverter.ToSingle(_binary.FieldData, dataAsInt32 + 12);
            parent.SetVector4(label, new(x, y, z, w));
        }
    }
}
