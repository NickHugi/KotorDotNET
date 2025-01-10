using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kotor.NET.Resources.Kotor2DA.Events;

public class TwoDAColumnChangedEventArgs : EventArgs
{
    public TwoDAColumnAction Action { get; set; }
    public string ColumnHeader { get; set; }

    public TwoDAColumnChangedEventArgs(TwoDAColumnAction action, string columnName)
    {
        Action = action;
        ColumnHeader = columnName;
    }
}
