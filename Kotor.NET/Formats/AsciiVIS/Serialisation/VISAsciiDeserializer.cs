using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Kotor.NET.Exceptions;
using Kotor.NET.Resources.KotorVIS;

namespace Kotor.NET.Formats.AsciiVIS.Serialisation;

public class VISAsciiDeserializer
{
    private VISAscii _ascii { get; }

    public VISAsciiDeserializer(VISAscii ascii)
    {
        _ascii = ascii;
    }

    public VIS Deserialize()
    {
        try
        {
            var vis = new VIS();

            _ascii.Rooms.ForEach(x => vis.Add(x.Model));

            foreach (var asciiRoom in _ascii.Rooms)
            {
                var room = vis.Get(asciiRoom.Model);
                asciiRoom.Visibility.ForEach(x => room.SetCanSee(x, true));
            }

            return vis;
        }
        catch (Exception e)
        {
            throw new DeserializationException("Failed to deserialize the VIS data.", e);
        }
    }
}
