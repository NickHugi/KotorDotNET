using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Kotor.NET.Exceptions;
using Kotor.NET.Resources.KotorLYT;

namespace Kotor.NET.Formats.AsciiLYT.Serialisation;

public class LYTAsciiDeserializer
{
    private LYTAscii _ascii { get; }

    public LYTAsciiDeserializer(LYTAscii ascii)
    {
        _ascii = ascii;
    }

    public LYT Deserialize()
    {
        try
        {
            var lyt = new LYT();

            foreach (var room in _ascii.Layout.Rooms)
            {
                lyt.Rooms.Add(room.Model, float.Parse(room.PositionX), float.Parse(room.PositionY), float.Parse(room.PositionZ));
            }
            foreach (var doorhook in _ascii.Layout.DoorHooks)
            {
                lyt.DoorHooks.Add(doorhook.Room, doorhook.Door, float.Parse(doorhook.PositionX), float.Parse(doorhook.PositionY), float.Parse(doorhook.PositionZ), float.Parse(doorhook.OrientationX), float.Parse(doorhook.OrientationY), float.Parse(doorhook.OrientationZ), float.Parse(doorhook.OrientationW));
            }
            foreach (var obstacle in _ascii.Layout.Obstacles)
            {
                lyt.Obstacles.Add(obstacle.Node, float.Parse(obstacle.PositionX), float.Parse(obstacle.PositionY), float.Parse(obstacle.PositionZ));
            }
            foreach (var track in _ascii.Layout.Tracks)
            {
                lyt.Tracks.Add(track.Node, float.Parse(track.PositionX), float.Parse(track.PositionY), float.Parse(track.PositionZ));
            }

            return lyt;
        }
        catch (Exception e)
        {
            throw new DeserializationException("Failed to deserialize the LYT data.", e);
        }
    }
}
