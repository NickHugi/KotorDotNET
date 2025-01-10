using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kotor.NET.Resources.Kotor2DA.Events;

public class TwoDARowChangedEventArgs : EventArgs
{
    public TwoDARowAction Action { get; set; }
    public string RowHeader { get; set; }
    public int RowIndex { get; set; }

    public TwoDARowChangedEventArgs(TwoDARowAction action, string rowHeader, int rowIndex)
    {
        Action = action;
        RowHeader = rowHeader;
        RowIndex = rowIndex;
    }
}
