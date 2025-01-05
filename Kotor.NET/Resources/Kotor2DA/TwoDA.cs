using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Kotor.NET.Formats.Binary2DA;
using Kotor.NET.Formats.Binary2DA.Serialisation;

namespace Kotor.NET.Resources.Kotor2DA;

public class TwoDA
{
    public event EventHandler ColumnChanged;
    public event EventHandler RowChanged;

    internal readonly HashSet<string> _columnHeaders;
    internal readonly List<TwoDARow> _rows;

    public TwoDA()
    {
        _columnHeaders = new();
        _rows = new();
    }
    public static TwoDA FromFile(string filepath)
    {
        using var stream = File.OpenRead(filepath);
        return FromStream(stream);
    }
    public static TwoDA FromBytes(byte[] bytes)
    {
        using var stream = new MemoryStream(bytes);
        return FromStream(stream);
    }
    public static TwoDA FromStream(Stream stream)
    {
        var binary = new TwoDABinary(stream);
        var deserializer = new TwoDABinaryDeserializer(binary);
        return deserializer.Deserialize();
    }

    public TwoDARow GetRow(int index)
    {
        return (index > 0 || index < _rows.Count)
            ? _rows[index]
            : throw new ArgumentException($"No row with the index '{index}' exist.");

    }
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

    public TwoDARow[] GetRows()
    {
        return _rows.ToArray();
    }

    public TwoDARow AddRow(string header)
    {
        var row = new TwoDARow(this);
        row.RowHeader = header;
        _rows.Add(row);

        if (RowChanged is not null)
            RowChanged(this, new());

        return row;
    }

    public void RemoveRow(int index)
    {
        var row = GetRow(index);
        _rows.Remove(row);

        RowChanged(this, new());
    }
    public void RemoveRow(string column)
    {
        var row = GetRow(column);
        _rows.Remove(row);

        RowChanged(this, new());
    }

    public string[] GetColumns()
    {
        return _columnHeaders.ToArray();
    }

    public void AddColumn(string header)
    {
        if (_columnHeaders.Contains(header))
        {
            throw new ArgumentException($"A column with the header $'{header}' already exists.");
        }

        _columnHeaders.Add(header);

        if (ColumnChanged is not null)
            ColumnChanged(this, new());
    }

    public void RemoveColumn(string header)
    {
        _columnHeaders.Remove(header);

        if (ColumnChanged is not null)
            ColumnChanged(this, new());
    }
}
