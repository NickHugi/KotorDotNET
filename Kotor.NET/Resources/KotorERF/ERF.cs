using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Kotor.NET.Common.Data;
using Kotor.NET.Formats.BinaryERF;
using Kotor.NET.Formats.BinaryERF.Serialisation;

namespace Kotor.NET.Resources.KotorERF;

/// <summary>
/// Represents the data of the ERF file format.
/// </summary>
public class ERF : IEnumerable<ERFResource>
{
    internal List<ERFResource> _resources;

    /// <summary>
    /// Initializes a new ERF table that contains no resources.
    /// </summary>
    public ERF()
    {
        _resources = new();
    }

    /// <summary>
    /// Creates a ERF object from the specified file.
    /// </summary>
    /// <param name="filepath">The path of the file to load the ERF data from.</param>
    /// <returns>The ERF object.</returns>
    /// <exception cref="ArgumentException"><paramref name="filepath" /> is a zero-length string, or contains one or more invalid characters.</exception>
    /// <exception cref="ArgumentNullException"><paramref name="filepath" /> is <see langword="null" />.</exception>
    /// <exception cref="FileNotFoundException">Thrown if the specified file does not exist.</exception>
    /// <exception cref="UnauthorizedAccessException">Thrown if the caller does not have the required permission to access the file.</exception>
    /// <exception cref="IOException">Thrown if there is an I/O error while opening the file.</exception>
    /// <exception cref="DeserializationException">Thrown if the data in the file is not formatted correctly.</exception>
    public static ERF FromFile(string filepath)
    {
        using var stream = File.OpenRead(filepath);
        return FromStream(stream);
    }
    /// <summary>
    /// Creates a ERF object from the specified byte array.
    /// </summary>
    /// <param name="bytes">The byte array containing the data of the ERF file.</param>
    /// <returns>The ERF object.</returns>
    /// <exception cref="ArgumentNullException"><paramref name="bytes" /> is <see langword="null" />.</exception>
    /// <exception cref="DeserializationException">Thrown if the data in the byte array is not formatted correctly.</exception>
    public static ERF FromBytes(byte[] bytes)
    {
        using var stream = new MemoryStream(bytes);
        return FromStream(stream);
    }
    /// <summary>
    /// Creates a ERF object from the specified stream.
    /// </summary>
    /// <param name="stream">The stream linked to the ERF file.</param>
    /// <returns>The ERF object.</returns>
    /// <exception cref="DeserializationException">Thrown if the data in the stream is not formatted correctly.</exception>
    public static ERF FromStream(Stream stream)
    {
        var binary = new ERFBinary(stream);
        var deserializer = new ERFBinaryDeserializer(binary);
        return deserializer.Deserialize();
    }

    /// <summary>
    /// Returns an enumerator that iterates through the resources stored in the ERF.
    /// </summary>
    /// <returns>An enumerator for the ERF.</returns>
    public IEnumerator<ERFResource> GetEnumerator() => _resources.GetEnumerator();
    /// <summary>
    /// Returns an enumerator that iterates through the resources stored in the ERF.
    /// </summary>
    /// <returns>An enumerator for the ERF.</returns>
    IEnumerator IEnumerable.GetEnumerator() => _resources.GetEnumerator();

    /// <summary>
    /// Adds a new resource to the ERF with the specified resource reference, type, and data.
    /// </summary>
    /// <param name="resref">The ResRef associated with the resource.</param>
    /// <param name="type">The file type of the resource.</param>
    /// <param name="data">The data associated with the resource.</param>
    /// <returns>The newly added <see cref="ERFResource"/>.</returns>
    public ERFResource Add(ResRef resref, ResourceType type, byte[] data)
    {
        var resource = new ERFResource(this, resref, type, data);
        _resources.Add(resource);
        return resource;
    }
}
