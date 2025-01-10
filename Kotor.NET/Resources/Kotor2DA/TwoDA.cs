using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Kotor.NET.Formats.Binary2DA;
using Kotor.NET.Formats.Binary2DA.Serialisation;
using Kotor.NET.Resources.Kotor2DA.Events;

namespace Kotor.NET.Resources.Kotor2DA;

public class TwoDA
{
    public event EventHandler<TwoDAColumnChangedEventArgs> ColumnChanged;
    public event EventHandler<TwoDARowChangedEventArgs> RowChanged;

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

        RowChanged.Invoke(this, new(TwoDARowAction.Added, row.RowHeader, row.Index));

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
        var index = row.Index;
        _rows.Remove(row);

        RowChanged.Invoke(this, new(TwoDARowAction.Removed, row.RowHeader, index));
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

        ColumnChanged.Invoke(this, new(TwoDAColumnAction.Added, header));
    }

    public void RemoveColumn(string header)
    {
        _columnHeaders.Remove(header);

        ColumnChanged.Invoke(this, new(TwoDAColumnAction.Removed, header));
    }
}
