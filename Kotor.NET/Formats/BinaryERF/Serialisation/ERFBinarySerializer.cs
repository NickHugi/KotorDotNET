using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Kotor.NET.Exceptions;
using Kotor.NET.Resources.KotorERF;

namespace Kotor.NET.Formats.BinaryERF.Serialisation;

public class ERFBinarySerializer
{
    private ERF _erf { get; }

    public ERFBinarySerializer(ERF erf)
    {
        _erf = erf;
    }

    public ERFBinary Serialize()
    {
        try
        {
            var binary = new ERFBinary();

            binary.FileHeader.FileType = ERFBinaryFileHeader.FILE_TYPES[0];
            binary.FileHeader.FileVersion = ERFBinaryFileHeader.FILE_VERSION;

            foreach (var resource in _erf)
            {
                binary.KeyEntries.Add(new()
                {
                    ResRef = resource.ResRef,
                    ResID = (uint)resource.Index,
                    ResType = (ushort)resource.Type.ID,
                });

                binary.ResourceData.Add(resource.Data);
            }

            binary.Recalculate();

            return binary;
        }
        catch (Exception e)
        {
            throw new SerializationException("Failed to serialize the ERF data.", e);
        }
    }
}
