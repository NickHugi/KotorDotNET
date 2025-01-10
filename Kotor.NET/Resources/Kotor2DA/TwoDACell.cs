using Kotor.NET.Resources.Kotor2DA.Events;

namespace Kotor.NET.Resources.Kotor2DA;

public class TwoDACell
{
    public event EventHandler<TwoDACellChangedEventArgs> CellChanged;

    internal readonly TwoDA _twoda;
    internal readonly TwoDARow _row;
    internal readonly string _column;

    internal TwoDACell(TwoDA twoda, TwoDARow row, string column)
    {
        _twoda = twoda;
        _row = row;
        _column = column;
    }

    public int RowIndex => _row.Index;
    public string RowHeader => _row.RowHeader;
    public string ColumnHeader => _column;

    public TwoDACell SetString(string? value)
    {
        _row._cells[_column] = value ?? "";

        EmitCellChanged();

        return this;
    }
    public string AsString()
    {
        return _row._cells.TryGetValue(_column, out var value) ? value : "";
    }

    public TwoDACell SetInt(int? value)
    {
        _row._cells[_column] = value?.ToString() ?? "";

        EmitCellChanged();

        return this;
    }
    public int? AsInt()
    {
        return int.TryParse(AsString(), out var result) ? result : null;
    }

    public TwoDACell SetDecimal(decimal? value)
    {
        _row._cells[_column] = value?.ToString() ?? "";

        EmitCellChanged();

        return this;
    }
    public decimal? AsDecimal()
    {
        return int.TryParse(AsString(), out var result) ? result : null;
    }

    public TwoDACell SetBool(bool? value)
    {
        _row._cells[_column] = value switch
        {
            true => "1",
            false => "0",
            _ => ""
        };

        EmitCellChanged();

        return this;
    }
    public bool? AsBool()
    {
        return AsString() switch
        {
            "1" => true,
            "0" => false,
            "" => null
        };
    }

    private void EmitCellChanged()
    {
        CellChanged.Invoke(this, new(_row.RowHeader, _column, AsString()));
    }
}
