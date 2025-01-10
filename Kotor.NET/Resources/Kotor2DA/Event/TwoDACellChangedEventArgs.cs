using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kotor.NET.Resources.Kotor2DA.Events;

public class TwoDACellChangedEventArgs : EventArgs
{
    public string RowHeader { get; set; }
    public string ColumnHeader { get; set; }
    public string CellValue { get; set; }

    public TwoDACellChangedEventArgs(string rowHeader, string columnHeader, string cellValue)
    {
        ColumnHeader = columnHeader;
        RowHeader = rowHeader;
        CellValue = cellValue;
    }
}
