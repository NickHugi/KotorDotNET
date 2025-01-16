using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Kotor.NET.Common.Data;
using Kotor.NET.Exceptions;
using Kotor.NET.Resources.KotorLYT;

namespace Kotor.NET.Formats.AsciiLYT.Serialisation;

public class LYTAsciiSerializer
{
    private LYT _lyt { get; }

    public LYTAsciiSerializer(LYT lyt)
    {
        _lyt = lyt;
    }

    public LYTAscii Serialize()
    {
        try
        {
            var ascii = new LYTAscii();

            foreach (var room in _lyt.Rooms)
            {
                ascii.Layout.Rooms.Add(new LYTAsciiRoom()
                {
                    Model = room.Model.ToString(),
                    PositionX = room.Position.X.ToString(),
                    PositionY = room.Position.Y.ToString(),
                    PositionZ = room.Position.Z.ToString(),
                });
            }

            foreach (var room in _lyt.DoorHooks)
            {
                ascii.Layout.DoorHooks.Add(new LYTAsciiDoorHook()
                {
                    Room = room.Room.ToString(),
                    Unknown = "0",
                    Door = room.Door.ToString(),
                    PositionX = room.Position.X.ToString(),
                    PositionY = room.Position.Y.ToString(),
                    PositionZ = room.Position.Z.ToString(),
                    OrientationX = room.Orientation.X.ToString(),
                    OrientationY = room.Orientation.Y.ToString(),
                    OrientationZ = room.Orientation.Z.ToString(),
                    OrientationW = room.Orientation.W.ToString(),
                });
            }

            foreach (var room in _lyt.Tracks)
            {
                ascii.Layout.Tracks.Add(new LYTAsciiTrack()
                {
                    Node = room.Room.ToString(),
                    PositionX = room.Position.X.ToString(),
                    PositionY = room.Position.Y.ToString(),
                    PositionZ = room.Position.Z.ToString(),
                });
            }

            foreach (var room in _lyt.Obstacles)
            {
                ascii.Layout.Obstacles.Add(new LYTAsciiObstacle()
                {
                    Node = room.Room.ToString(),
                    PositionX = room.Position.X.ToString(),
                    PositionY = room.Position.Y.ToString(),
                    PositionZ = room.Position.Z.ToString(),
                });
            }

            return ascii;
        }
        catch (Exception e)
        {
            throw new SerializationException("Failed to serialize the LYT data.", e);
        }
    }
}
