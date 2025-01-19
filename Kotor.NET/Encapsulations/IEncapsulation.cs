using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Kotor.NET.Common.Data;

namespace Kotor.NET.Tests.Encapsulation;

public interface IEncapsulation : IEnumerable<ResourceInfo>
{
    /// <summary>
    /// Path to where the resources are being stored.
    /// </summary>
    public string Path { get; }

    /// <summary>
    /// The number of resources that are stored at the Path.
    /// </summary>
    public int Count { get; }

    /// <summary>
    /// Reloads the resource table.
    /// </summary>
    public void Reload();

    /// <summary>
    /// Finds the resource with the specified ResRef and Resource Type.
    /// </summary>
    /// <param name="resref">The ResRef of the resource.</param>
    /// <param name="type">The resource type</param>
    /// <returns>A <see cref="ResourceInfo"/> object for the specified resource containing data on the what and where.</returns>
    public ResourceInfo Find(string resref, ResourceType type);

    /// <summary>
    /// Returns the data for the resource with the specified ResRef and Resource Type.
    /// </summary>
    /// <param name="resref">The ResRef of the resource.</param>
    /// <param name="type">The resource type</param>
    /// <returns>The data for the specified resource.</returns>
    public byte[] Read(string resref, ResourceType type);

    /// <summary>
    /// Write or overwrite existing data for the resource with the specified ResRef and Resource Type.
    /// </summary>
    /// <param name="resref">The ResRef of the resource.</param>
    /// <param name="type">The resource type</param>
    /// <param name="data">The data to write.</param>
    public void Write(string resref, ResourceType type, byte[] data);

    /// <summary>
    /// Deletes the resource at the path that matches the specified ResRef and Resource Type.
    /// </summary>
    /// <param name="resref">The ResRef of the resource.</param>
    /// <param name="type">The resource type</param>
    public void Delete(string resref, ResourceType type);
}
