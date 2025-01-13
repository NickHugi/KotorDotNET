namespace Kotor.NET.Resources.Kotor2DA;

/// <summary>
/// Represents a row contained within a 2DA table. 
/// </summary>
public class TwoDARow
{
    /// <summary>
    /// The string identifier for the row within the 2DA table. This should be unique to the table.
    /// </summary>
    public string RowHeader { get; set; }
    /// <summary>
    /// Gets the index of the row into the 2DA table.
    /// </summary>
    public int Index => _twoda._rows.IndexOf(this);

    internal readonly Dictionary<string, string> _cells;
    internal readonly TwoDA _twoda;

    internal TwoDARow(TwoDA twoda)
    {
        _twoda = twoda;
        _cells = new Dictionary<string, string>();
        RowHeader = "";
    }

    /// <summary>
    /// Retrieves a specific cell under a given column header for this row.
    /// </summary>
    /// <param name="columnHeader">The column header of which the cell is located under.</param>
    /// <returns>The cell corresponding to the specified column header for this row.</returns>
    public TwoDACell GetCell(string columnHeader)
    {
        return new(_twoda, this, columnHeader);
    }
}
