using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Kotor.NET.Common.Data;
using Kotor.NET.Formats.BinaryBIF;
using Kotor.NET.Formats.BinaryKEY;

namespace Kotor.NET.Tests.Encapsulation;

public class KEYEncapsulation : IEncapsulation
{
    public string Path { get; private set; }
    public int Count { get; private set; }

    private List<ResourceInfo> _resources;

    public KEYEncapsulation(string filepath)
    {
        Path = filepath;
        _resources = default!;

        Reload();
    }

    public void Reload()
    {
        _resources = new();

        using var keyStream = File.Open(Path, FileMode.Open, FileAccess.Read);
        var keyBinary = new KEYBinary(keyStream);

        // For performance reasons, we build a hash table for when we do key table lookups
        var hashmap = new Dictionary<uint, KEYBinaryKeyEntry>();
        keyBinary.Keys.ForEach(resource => hashmap.Add(resource.ResourceID, resource));

        foreach (var bifRelativePath in keyBinary.Filenames)
        {
            var bifPath = System.IO.Path.Join(System.IO.Path.GetDirectoryName(Path), bifRelativePath);

            using var bifStream = File.Open(bifPath, FileMode.Open, FileAccess.Read);
            var bifBinary = new BIFBinary(bifStream);

            _resources.AddRange(bifBinary.Resources.Select((entry, index) =>
            {
                var key = hashmap[entry.ResourceID];

                return new ResourceInfo
                {
                    FilePath = bifPath,
                    ResRef = key.ResRef.Get(),
                    Type = ResourceType.ByID(key.ResourceType),
                    Offset = entry.Offset,
                    Size = entry.FileSize,
                };
            }));
        }
    }

    public byte[] Read(string resref, ResourceType type)
    {
        var resource = Find(resref, type);

        using var stream = new BinaryReader(File.Open(Path, FileMode.Open, FileAccess.ReadWrite));
        stream.BaseStream.Seek(resource.Offset, SeekOrigin.Begin);
        return stream.ReadBytes(resource.Size);
    }

    public void Write(string resref, ResourceType type, byte[] data)
    {
        throw new NotImplementedException("Writing to KEY/BIF files is not supported.");
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
        throw new NotImplementedException("Writing to KEY/BIF files is not supported.");
    }

    public IEnumerator<ResourceInfo> GetEnumerator() => _resources.GetEnumerator();
    IEnumerator IEnumerable.GetEnumerator() => _resources.GetEnumerator();
}
