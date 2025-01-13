using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Kotor.NET.Exceptions;
using Kotor.NET.Formats.Binary2DA;
using Kotor.NET.Formats.Binary2DA.Serialisation;

namespace Kotor.NET.Resources.Kotor2DA;

/// <summary>
/// Represents the data of the 2DA file format.
/// </summary>
public class TwoDA
{
    internal readonly HashSet<string> _columnHeaders;
    internal readonly List<TwoDARow> _rows;

    /// <summary>
    /// Initializes a new 2DA table that contains no rows or columns.
    /// </summary>
    public TwoDA()
    {
        _columnHeaders = new();
        _rows = new();
    }

    /// <summary>
    /// Creates a 2DA object from the specified file.
    /// </summary>
    /// <param name="filepath">The path of the file to load the 2DA data from.</param>
    /// <returns>The 2DA object.</returns>
    /// <exception cref="ArgumentException"><paramref name="filepath" /> is a zero-length string, or contains one or more invalid characters.</exception>
    /// <exception cref="ArgumentNullException"><paramref name="filepath" /> is <see langword="null" />.</exception>
    /// <exception cref="FileNotFoundException">Thrown if the specified file does not exist.</exception>
    /// <exception cref="UnauthorizedAccessException">Thrown if the caller does not have the required permission to access the file.</exception>
    /// <exception cref="IOException">Thrown if there is an I/O error while opening the file.</exception>
    /// <exception cref="DeserializationException">Thrown if the data in the file is not formatted correctly.</exception>
    public static TwoDA FromFile(string filepath)
    {
        using var stream = File.OpenRead(filepath);
        return FromStream(stream);
    }
    /// <summary>
    /// Creates a 2DA object from the specified byte array.
    /// </summary>
    /// <param name="bytes">The byte array containing the data of the 2DA file.</param>
    /// <returns>The 2DA object.</returns>
    /// <exception cref="ArgumentNullException"><paramref name="bytes" /> is <see langword="null" />.</exception>
    /// <exception cref="DeserializationException">Thrown if the data in the byte array is not formatted correctly.</exception>
    public static TwoDA FromBytes(byte[] bytes)
    {
        using var stream = new MemoryStream(bytes);
        return FromStream(stream);
    }
    /// <summary>
    /// Creates a 2DA object from the specified stream.
    /// </summary>
    /// <param name="stream">The stream linked to the 2DA file.</param>
    /// <returns>The 2DA object.</returns>
    /// <exception cref="DeserializationException">Thrown if the data in the stream is not formatted correctly.</exception>
    public static TwoDA FromStream(Stream stream)
    {
        var binary = new TwoDABinary(stream);
        var deserializer = new TwoDABinaryDeserializer(binary);
        return deserializer.Deserialize();
    }

    /// <summary>
    /// Returns the row object at the specified index in the table.
    /// </summary>
    /// <param name="index">The index into the table to retrieve.</param>
    /// <returns>The row object at the specified index in the table.</returns>
    /// <exception cref="ArgumentOutOfRangeException">Thrown if an invalid row index is provided.</exception>
    public TwoDARow GetRow(int index)
    {
        return (index > 0 || index < _rows.Count)
            ? _rows[index]
            : throw new ArgumentOutOfRangeException($"No row with the index '{index}' exist.");

    }
    /// <summary>
    /// Returns the row object that has the specified header in the table.
    /// </summary>
    /// <param name="header">The header of the row to retrieve.</param>
    /// <returns>The row with the specified header in the table.</returns>
    /// <exception cref="ArgumentException">A row with the specified header does not exist or multiple rows with the same header exists.</exception>
    public TwoDARow GetRow(string header)
    {
        _rows.Where(x => x.RowHeader == header).ToList();

        if (_rows.Count == 0)
        {
            throw new ArgumentException($"No row with header '{header}' exists.");
        }
        else if (_rows.Count == 1)
        {
            return _rows.First();
        }
        else
        {
            throw new ArgumentException($"Multiple rows with the header '{header}' exist.");
        }
    }

    /// <summary>
    /// Returns an array of all currently existing rows in the table.
    /// </summary>
    /// <returns>The current rows in the 2DA object.</returns>
    public TwoDARow[] GetRows()
    {
        return _rows.ToArray();
    }

    /// <summary>
    /// Adds a new row into the table with the specified row header.
    /// </summary>
    /// <param name="header">The header to be assigned for the new row.</param>
    /// <returns>The new row added to the table.</returns>
    public TwoDARow AddRow(string header)
    {
        var row = new TwoDARow(this);
        row.RowHeader = header;
        _rows.Add(row);
        return row;
    }

    /// <summary>
    /// Removes the row from the table at the specified index.
    /// </summary>
    /// <param name="index">The index into the table to remove.</param>
    /// <exception cref="ArgumentOutOfRangeException">Thrown if an invalid row index is provided.</exception>
    public void RemoveRow(int index)
    {
        var row = GetRow(index);
        _rows.Remove(row);
    }
    /// <summary>
    /// Removes the row from the table with the specified row header.
    /// </summary>
    /// <param name="header">The header of the row to be removed.</param>
    /// <exception cref="ArgumentException">A row with the specified header does not exist or multiple rows with the same header exists.</exception>
    public void RemoveRow(string header)
    {
        var row = GetRow(header);
        _rows.Remove(row);
    }

    /// <summary>
    /// Returns the array of columns, in order, stored inside the table.
    /// </summary>
    /// <returns>The ordered array of columns in the table.</returns>
    public string[] GetColumns()
    {
        return _columnHeaders.ToArray();
    }

    /// <summary>
    /// Adds a new column into the table with the specified column header.
    /// </summary>
    /// <param name="header">The header to be assigned to the new column.</param>
    /// <exception cref="ArgumentException">Thrown if a column with the same header already exists.</exception>
    public void AddColumn(string header)
    {
        if (_columnHeaders.Contains(header))
        {
            throw new ArgumentException($"A column with the header $'{header}' already exists.");
        }

        _columnHeaders.Add(header);
    }

    /// <summary>
    /// Removes the column in the table with the specified header. If no header exists, no exception is thrown.
    /// </summary>
    /// <param name="header">The header to be assigned to the new column.</param>
    public void RemoveColumn(string header)
    {
        _columnHeaders.Remove(header);
    }
}
