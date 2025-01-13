using Kotor.NET.Common.Data;

namespace Kotor.NET.Resources.KotorERF;

public class ERFResource
{
    /// <summary>
    /// The resource identifier.
    /// </summary>
    public ResRef ResRef { get; set; }
    /// <summary>
    /// The file type of the resource.
    /// </summary>
    public ResourceType Type { get; set; }
    /// <summary>
    /// The data of the resource.
    /// </summary>
    public byte[] Data { get; set; }
    /// <summary>
    /// Gets the size of the resource in bytes.
    /// </summary>
    public int Size => Data.Length;
    /// <summary>
    /// Gets the index of the resource into the ERF.
    /// </summary>
    public int Index => _erf._resources.IndexOf(this);

    private ERF _erf;

    internal ERFResource(ERF erf, ResRef resref, ResourceType resourceType, byte[] data)
    {
        _erf = erf;
        ResRef = resref;
        Type = resourceType;
        Data = data;
    }

    /// <summary>
    /// Removes this resource from the ERF.
    /// </summary>
    public void Remove()
    {
        _erf._resources.Remove(this);
    }
}
