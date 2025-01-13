using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Kotor.NET.Exceptions;
using Kotor.NET.Resources.KotorTPC;
using Kotor.NET.Resources.KotorTPC.TextureFormats;

namespace Kotor.NET.Formats.BinaryTPC.Serialisation;

public class TPCBinarySerializer
{
    private TPC _tpc { get; }

    public TPCBinarySerializer(TPC tpc)
    {
        _tpc = tpc;
    }

    public TPCBinary Serialize()
    {
        try
        {
            var binary = new TPCBinary();

            binary.FileHeader.Width = _tpc.Width;
            binary.FileHeader.Height = _tpc.Height;
            binary.FileHeader.MipmapCount = _tpc.MipmapCount;
            binary.FileHeader.Encoding = (byte)_tpc.TextureFormat.BinaryEncoding;

            binary.Layers = Enumerable.Range(0, _tpc.LayerCount).Select(x => new TPCBinaryLayer()).ToList();

            for (int i = 0; i < _tpc.MipmapCount; i++)
            {
                for (int j = 0; j < _tpc.LayerCount; j++)
                {
                    var layer = _tpc.Layer[j];
                    var mipmap = layer._mipmaps[i];
                    var size = _tpc._textureFormat.GetDataSize(mipmap.Width, mipmap.Height);

                    binary.Layers[j].Mipmaps.Add(mipmap.Data);
                }
            }

            binary.Recalculate();

            return binary;
        }
        catch (Exception e)
        {
            throw new SerializationException("Failed to serialize the TPC data.", e);
        }
    }
}
