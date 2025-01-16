using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Kotor.NET.Common.Data;
using Kotor.NET.Common;
using Kotor.NET.Exceptions;
using Kotor.NET.Formats.BinaryGFF;
using Kotor.NET.Helpers;
using Kotor.NET.Resources.KotorGFF;

namespace Kotor.NET.Formats.Binary2DA.Serialisation;

public class GFFBinarySerializer
{
    private GFF _gff { get; }
    private GFFBinary _binary;

    public GFFBinarySerializer(GFF gff)
    {
        _gff = gff;
    }

    public GFFBinary Serialize()
    {
        try
        {
            _binary = new GFFBinary();

            _binary.FileHeader.FileType = _gff.Type.ToString().ToUpper().PadRight(4, ' ');
            UnparseStruct(_gff.Root);
            _binary.Recalculate();

            return _binary;
        }
        catch (Exception e)
        {
            throw new SerializationException("Failed to serialize the 2DA data.", e);
        }
    }

    private int UnparseStruct(GFFStruct gffStruct)
    {
        var structIndex = _binary.Structs.Count();

        _binary.Structs.Add(new()
        {
            ID = gffStruct.ID,
            FieldCount = (uint)gffStruct.FieldCount()
        });

        var fieldIndices = new List<int>();
        foreach (var item in gffStruct._data)
        {
            var index = UnparseField(gffStruct, item.Key);
            fieldIndices.Add(index);
        }

        if (gffStruct.FieldCount() == 1)
        {
            _binary.Structs[structIndex].DataOrDataOffset = fieldIndices.Last();
        }
        else if (gffStruct.FieldCount() > 1)
        {
            _binary.Structs[structIndex].DataOrDataOffset = _binary.FieldIndices.Count();
            _binary.FieldIndices = _binary.FieldIndices.Concat(fieldIndices.SelectMany(x => BitConverter.GetBytes(x))).ToArray();
        }

        return structIndex;
    }
    private int UnparseField(GFFStruct gffStruct, string fieldName)
    {
        var labelIndex = _binary.Labels.Count();
        _binary.Labels.Add(fieldName);

        GFFBinaryFieldType fieldType;
        byte[] dataOrDataOffset = new byte[0];

        if (gffStruct._data[fieldName] is byte uint8)
        {
            fieldType = GFFBinaryFieldType.UInt8;
            dataOrDataOffset = BitConverter.GetBytes((uint)uint8);
        }
        else if (gffStruct._data[fieldName] is sbyte int8)
        {
            fieldType = GFFBinaryFieldType.Int8;
            dataOrDataOffset = BitConverter.GetBytes((int)int8);
        }
        else if (gffStruct._data[fieldName] is ushort uint16)
        {
            fieldType = GFFBinaryFieldType.UInt16;
            dataOrDataOffset = BitConverter.GetBytes((uint)uint16);
        }
        else if (gffStruct._data[fieldName] is short int16)
        {
            fieldType = GFFBinaryFieldType.Int16;
            dataOrDataOffset = BitConverter.GetBytes((int)int16);
        }
        else if (gffStruct._data[fieldName] is uint uint32)
        {
            fieldType = GFFBinaryFieldType.UInt32;
            dataOrDataOffset = BitConverter.GetBytes((uint)uint32);
        }
        else if (gffStruct._data[fieldName] is int int32)
        {
            fieldType = GFFBinaryFieldType.Int32;
            dataOrDataOffset = BitConverter.GetBytes((int)int32);
        }
        else if (gffStruct._data[fieldName] is ulong uint64)
        {
            fieldType = GFFBinaryFieldType.UInt64;
            var offsetToFieldData = _binary.FieldData.Count();
            dataOrDataOffset = BitConverter.GetBytes(offsetToFieldData);
            _binary.FieldData = _binary.FieldData.Concat(BitConverter.GetBytes(uint64)).ToArray();
        }
        else if (gffStruct._data[fieldName] is long int64)
        {
            fieldType = GFFBinaryFieldType.Int64;
            var offsetToFieldData = _binary.FieldData.Count();
            dataOrDataOffset = BitConverter.GetBytes(offsetToFieldData);
            _binary.FieldData = _binary.FieldData.Concat(BitConverter.GetBytes(int64)).ToArray();
        }
        else if (gffStruct._data[fieldName] is float singlef)
        {
            fieldType = GFFBinaryFieldType.Single;
            dataOrDataOffset = BitConverter.GetBytes(singlef);
        }
        else if (gffStruct._data[fieldName] is double doublef)
        {
            fieldType = GFFBinaryFieldType.Double;
            var offsetToFieldData = _binary.FieldData.Count();
            dataOrDataOffset = BitConverter.GetBytes(offsetToFieldData);
            _binary.FieldData = _binary.FieldData.Concat(BitConverter.GetBytes(doublef)).ToArray();
        }
        else if (gffStruct._data[fieldName] is string text)
        {
            fieldType = GFFBinaryFieldType.String;
            var offsetToFieldData = _binary.FieldData.Count();
            dataOrDataOffset = BitConverter.GetBytes(offsetToFieldData);
            _binary.FieldData = _binary.FieldData.Concat(BitConverter.GetBytes(text.Length)).Concat(KEncoding.GetWindows1252().GetBytes(text)).ToArray();
        }
        else if (gffStruct._data[fieldName] is ResRef resref)
        {
            fieldType = GFFBinaryFieldType.ResRef;
            var offsetToFieldData = _binary.FieldData.Count();
            dataOrDataOffset = BitConverter.GetBytes(offsetToFieldData);
            _binary.FieldData = _binary.FieldData.Concat(new byte[] { (byte)resref.Length() }).Concat(KEncoding.GetWindows1252().GetBytes(resref.Get())).ToArray();
        }
        else if (gffStruct._data[fieldName] is LocalisedString locstring)
        {
            fieldType = GFFBinaryFieldType.LocalisedString;
            var offsetToFieldData = _binary.FieldData.Count();
            dataOrDataOffset = BitConverter.GetBytes(offsetToFieldData);
            var totalSize = 12 + locstring.AllSubstrings().Sum(x => x.Text.Length + 8);
            _binary.FieldData = _binary.FieldData
                .Concat(BitConverter.GetBytes(totalSize))
                .Concat(BitConverter.GetBytes(locstring.StringRef))
                .Concat(BitConverter.GetBytes(locstring.Count()))
                .ToArray();
            locstring.AllSubstrings().ToList().ForEach(x => _binary.FieldData = _binary.FieldData
                .Concat(BitConverter.GetBytes(x.StringID))
                .Concat(BitConverter.GetBytes(x.Text.Length))
                .Concat(KEncoding.GetWindows1252().GetBytes(x.Text))
                .ToArray());
        }
        else if (gffStruct._data[fieldName] is byte[] data)
        {
            fieldType = GFFBinaryFieldType.Binary;
            var offsetToFieldData = _binary.FieldData.Count();
            dataOrDataOffset = BitConverter.GetBytes(offsetToFieldData);
            _binary.FieldData = _binary.FieldData.Concat(BitConverter.GetBytes(data.Length)).Concat(data).ToArray();
        }
        else if (gffStruct._data[fieldName] is GFFStruct child)
        {
            fieldType = GFFBinaryFieldType.Struct;
            var structIndex = UnparseStruct(child);
            dataOrDataOffset = BitConverter.GetBytes(structIndex);
        }
        else if (gffStruct._data[fieldName] is GFFList list)
        {
            fieldType = GFFBinaryFieldType.List;

            var indicies = new List<int>();
            for (int i = 0; i < list.Count; i++)
            {
                indicies.Add(UnparseStruct(list.ElementAt(i)));
            }

            dataOrDataOffset = BitConverter.GetBytes(_binary.ListIndices.Count());
            _binary.ListIndices = _binary.ListIndices.Concat(BitConverter.GetBytes(indicies.Count())).ToArray();
            foreach (var index in indicies)
            {
                _binary.ListIndices = _binary.ListIndices.Concat(BitConverter.GetBytes(index)).ToArray();
            }
        }
        else if (gffStruct._data[fieldName] is Vector3 vector3)
        {
            fieldType = GFFBinaryFieldType.Vector3;
            var offsetToFieldData = _binary.FieldData.Count();
            dataOrDataOffset = BitConverter.GetBytes(offsetToFieldData);
            _binary.FieldData = _binary.FieldData
                .Concat(BitConverter.GetBytes(vector3.X))
                .Concat(BitConverter.GetBytes(vector3.Y))
                .Concat(BitConverter.GetBytes(vector3.Z))
                .ToArray();
        }
        else if (gffStruct._data[fieldName] is Vector4 vector4)
        {
            fieldType = GFFBinaryFieldType.Vector4;
            var offsetToFieldData = _binary.FieldData.Count();
            dataOrDataOffset = BitConverter.GetBytes(offsetToFieldData);
            _binary.FieldData = _binary.FieldData
                .Concat(BitConverter.GetBytes(vector4.X))
                .Concat(BitConverter.GetBytes(vector4.Y))
                .Concat(BitConverter.GetBytes(vector4.Z))
                .Concat(BitConverter.GetBytes(vector4.W))
                .ToArray();
        }
        else
            throw new Exception("Illegal type got assigned to a GFF field somehow?");

        var fieldIndex = _binary.Fields.Count();
        _binary.Fields.Add(new()
        {
            LabelIndex = (uint)labelIndex,
            Type = (int)fieldType,
            DataOrDataOffset = dataOrDataOffset,
        });

        return fieldIndex;
    }
}
