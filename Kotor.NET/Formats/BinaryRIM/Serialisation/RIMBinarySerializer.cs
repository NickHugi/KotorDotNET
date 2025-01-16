using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using Kotor.NET.Resources.KotorRIM;

namespace Kotor.NET.Formats.BinaryRIM.Serialisation;

public class RIMBinarySerializer
{
    private RIM _rim { get; }

    public RIMBinarySerializer(RIM rim)
    {
        _rim = rim;
    }

    public RIMBinary Serialize()
    {
        try
        {
            var binary = new RIMBinary();

            binary.FileHeader.FileType = RIMBinaryFileHeader.FILE_TYPES[0];
            binary.FileHeader.FileVersion = RIMBinaryFileHeader.FILE_VERSION;

            foreach (var resource in _rim)
            {
                binary.ResourceEntries.Add(new()
                {
                    ResRef = resource.ResRef,
                    ResourceID = resource.Index,
                    ResourceTypeID = resource.Type.ID,
                });

                binary.ResourceData.Add(resource.Data);
            }

            binary.Recalculate();

            return binary;
        }
        catch (Exception e)
        {
            throw new SerializationException("Failed to serialize the RIM data.", e);
        }
    }
}
