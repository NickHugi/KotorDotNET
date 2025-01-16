using Kotor.NET.Resources.Kotor2DA.Events;

namespace Kotor.NET.Resources.Kotor2DA;

/// <summary>
/// Represents a cell within a 2DA table.
/// </summary>
public class TwoDACell
{
    internal readonly TwoDA _twoda;
    internal readonly TwoDARow _row;
    internal readonly string _column;

    internal TwoDACell(TwoDA twoda, TwoDARow row, string column)
    {
        _twoda = twoda;
        _row = row;
        _column = column;
    }

    /// <summary>
    /// Gets the index of the row this cell belongs to.
    /// </summary>
    public int RowIndex => _row.Index;
    /// <summary>
    /// Gets the header of the row this cell belongs to.
    /// </summary>
    public string RowHeader => _row.RowHeader;
    /// <summary>
    /// Gets the header of the column this cell belongs to.
    /// </summary>
    public string ColumnHeader => _column;

    /// <summary>
    /// Sets the value of this cell as a string.
    /// </summary>
    /// <param name="value">The value to set for the cell.</param>
    /// <returns>The updated cell.</returns>
    public TwoDACell SetString(string? value)
    {
        _row._cells[_column] = value ?? "";

        return this;
    }
    /// <summary>
    /// Gets the value of this cell as a string.
    /// </summary>
    /// <returns>The value of the cell as a string.</returns>
    public string AsString()
    {
        return _row._cells.TryGetValue(_column, out var value) ? value : "";
    }

    /// <summary>
    /// Sets the value of this cell as an integer.
    /// </summary>
    /// <param name="value">The integer value to set for the cell.</param>
    /// <returns>The updated cell.</returns>
    public TwoDACell SetInt(int? value)
    {
        _row._cells[_column] = value?.ToString() ?? "";

        return this;
    }
    /// <summary>
    /// Gets the value of this cell as an integer.
    /// </summary>
    /// <returns>The value of the cell as an integer, or <see langword="null"/> if not a valid integer.</returns>
    public int? AsInt()
    {
        return int.TryParse(AsString(), out var result) ? result : null;
    }

    /// <summary>
    /// Sets the value of this cell as a decimal.
    /// </summary>
    /// <param name="value">The decimal value to set for the cell.</param>
    /// <returns>The updated cell.</returns>
    public TwoDACell SetDecimal(decimal? value)
    {
        _row._cells[_column] = value?.ToString() ?? "";

        return this;
    }
    /// <summary>
    /// Gets the value of this cell as a decimal.
    /// </summary>
    /// <returns>The value of the cell as a decimal, or <see langword="null"/> if not a valid decimal.</returns>
    public decimal? AsDecimal()
    {
        return int.TryParse(AsString(), out var result) ? result : null;
    }

    /// <summary>
    /// Sets the value of this cell as a boolean.
    /// </summary>
    /// <param name="value">The boolean value to set for the cell.</param>
    /// <returns>The updated cell.</returns>
    public TwoDACell SetBool(bool? value)
    {
        _row._cells[_column] = value switch
        {
            true => "1",
            false => "0",
            _ => ""
        };

        return this;
    }
    /// <summary>
    /// Gets the value of this cell as a boolean.
    /// </summary>
    /// <returns>The value of the cell as a boolean, or <see langword="null"/> if not a valid boolean.</returns>
    public bool? AsBool()
    {
        return AsString() switch
        {
            "1" => true,
            "0" => false,
            _ => null
        };
    }
}
