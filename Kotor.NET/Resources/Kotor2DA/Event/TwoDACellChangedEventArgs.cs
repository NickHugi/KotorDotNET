using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kotor.NET.Resources.Kotor2DA.Events;

public class TwoDACellChangedEventArgs : EventArgs
{
    public int RowIndex { get; set; }
    public string RowHeader { get; set; }
    public string ColumnHeader { get; set; }
    public string CellValue { get; set; }

    public TwoDACellChangedEventArgs(int rowIndex, string rowHeader, string columnHeader, string cellValue)
    {
        RowIndex = rowIndex;
        ColumnHeader = columnHeader;
        RowHeader = rowHeader;
        CellValue = cellValue;
    }
}
