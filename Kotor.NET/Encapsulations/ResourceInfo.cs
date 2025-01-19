using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Kotor.NET.Common.Data;

namespace Kotor.NET.Tests.Encapsulation;

public class ResourceInfo
{
    public required string FilePath { get; init; }
    public required string ResRef { get; init; }
    public required ResourceType Type { get; init; }
    public required int Size { get; init; }
    public required int Offset { get; init; }

    public ResourceInfo()
    {
    }
    public ResourceInfo(string filePath, string resRef, ResourceType type, int size, int offset)
    {
        FilePath = filePath;
        ResRef = resRef;
        Type = type;
        Size = size;
        Offset = offset;
    }
}
