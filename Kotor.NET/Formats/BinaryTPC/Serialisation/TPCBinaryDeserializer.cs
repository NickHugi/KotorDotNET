using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Kotor.NET.Exceptions;
using Kotor.NET.Resources.KotorTPC;
using Kotor.NET.Resources.KotorTPC.Builder;
using Kotor.NET.Resources.KotorTPC.TextureFormats;

namespace Kotor.NET.Formats.BinaryTPC.Serialisation;

public class TPCBinaryDeserializer
{
    private TPCBinary _binary { get; }

    public TPCBinaryDeserializer(TPCBinary binary)
    {
        _binary = binary;
    }

    public TPC Deserialize()
    {
        try
        {
            var encoding = (TPCBinaryEncoding)_binary.FileHeader.Encoding;
            var compressed = _binary.FileHeader.Compressed;
            var mipmaps = _binary.FileHeader.MipmapCount;
            var width = _binary.FileHeader.Width;
            var height = _binary.FileHeader.CubeMap ? _binary.FileHeader.Width : _binary.FileHeader.Height;
            var layers = (byte)_binary.Layers.Count;

            return new TPCBuilder()
                .SetFormat(encoding, compressed)
                .SetDimensions(width, height)
                .SetLayerCount(layers)
                .SetMipmapCount(mipmaps)
                .AddLayer(layer => _binary.Layers.ForEach(binaryLayer =>
                {
                    layer.SetLayer(_binary.Layers.IndexOf(binaryLayer));
                    binaryLayer.Mipmaps.ForEach(binaryMipmap =>
                    {
                        layer.AddData(binaryMipmap, binaryLayer.Mipmaps.IndexOf(binaryMipmap));
                    });
                }))
                .Build();
        }
        catch (Exception e)
        {
            throw new DeserializationException("Failed to deserialize the TPC data.", e);
        }
    }
}
