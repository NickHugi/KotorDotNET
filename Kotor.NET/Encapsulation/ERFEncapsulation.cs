using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Kotor.NET.Common.Data;
using Kotor.NET.Formats.BinaryERF;

namespace Kotor.NET.Tests.Encapsulation;

public class ERFEncapsulation : IEncapsulatedFormat
{
    public string Path { get; private set; }
    public int Count { get; private set; }

    private List<ResourceInfo> _resources;

    public ERFEncapsulation(string filepath)
    {
        Path = filepath;
        _resources = default!;

        Reload();
    }

    public void Reload()
    {
        using var stream = File.OpenRead(Path);
        var binary = new ERFBinary(stream);

        Count = binary.ResourceEntries.Count;

        var file = binary.ResourceEntries.ToList();
        _resources = binary.KeyEntries.Select((key, index) => new ResourceInfo
        {
            FilePath = Path,
            ResRef = key.ResRef.Get(),
            Type = ResourceType.ByID((int)key.ResID),
            Offset = file.ElementAt(index).Offset,
            Size = file.ElementAt(index).Size,
        }).ToList();
    }

    public byte[] Read(string resref, ResourceType type)
    {
        var resource = Find(resref, type);

        using var stream = new BinaryReader(File.OpenRead(Path));
        stream.BaseStream.Seek(resource.Offset, SeekOrigin.Begin);
        return stream.ReadBytes(resource.Size);
    }

    public void Write(string resref, ResourceType type, byte[] data)
    {
        using var stream = File.Open(Path, FileMode.Open, FileAccess.ReadWrite);
        var binary = new ERFBinary(stream);

        binary.KeyEntries.Add(new()
        {
            ResRef = resref,
            ResType = (ushort)type.ID,
            ResID = (ushort)binary.ResourceData.Count,
        });
        binary.ResourceData.Add(data);

        binary.Recalculate();
        stream.SetLength(0);
        binary.Write(stream);
    }

    public ResourceInfo Find(string resref, ResourceType type)
    {
        var resource = this.FirstOrDefault(x => x.ResRef == resref && x.Type == type);
        if (resource is null)
            throw new ArgumentException($"Could not file resource '{resref}' of type '{type.Extension}'.");
        return resource;
    }

    public void Delete(string resref, ResourceType type)
    {
        using var stream = File.Open(Path, FileMode.Open, FileAccess.ReadWrite);
        var binary = new ERFBinary(stream);

        binary.KeyEntries.RemoveAll(x => x.ResRef.Get() == resref && x.ResType == (ushort)type.ID);

        binary.Recalculate();
        stream.SetLength(0);
        binary.Write(stream);
    }

    public IEnumerator<ResourceInfo> GetEnumerator() => _resources.GetEnumerator();
    IEnumerator IEnumerable.GetEnumerator() => _resources.GetEnumerator();
}
