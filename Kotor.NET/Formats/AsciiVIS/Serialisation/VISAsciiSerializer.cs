using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Kotor.NET.Common.Data;
using Kotor.NET.Exceptions;
using Kotor.NET.Resources.KotorVIS;

namespace Kotor.NET.Formats.AsciiVIS.Serialisation;

public class VISAsciiSerializer
{
    private VIS _vis { get; }

    public VISAsciiSerializer(VIS vis)
    {
        _vis = vis;
    }

    public VISAscii Serialize()
    {
        try
        {
            var ascii = new VISAscii();

            foreach (var room in _vis)
            {
                _vis._visibility.Select(x => new VISAsciiRoom
                {
                    Model = x.Key,
                    Visibility = x.Value.ToList(),
                });
            }

            return ascii;
        }
        catch (Exception e)
        {
            throw new SerializationException("Failed to serialize the VIS data.", e);
        }
    }
}
