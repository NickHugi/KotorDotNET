using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Kotor.DevelopmentKit.Base.Common;
using Kotor.DevelopmentKit.Editor2DA.ViewModels;

namespace Kotor.DevelopmentKit.Editor2DA.Actions;

public class EditCellAction : IAction<TwoDAResourceEditorViewModel>
{
    public int RowID { get; }
    public string ColumnHeader { get; }
    public string NewValue { get; }
    public string OldValue { get; }

    public EditCellAction(int rowID, string columnHeader, string newValue, string oldValue)
    {
        RowID = rowID;
        ColumnHeader = columnHeader;
        NewValue = newValue;
        OldValue = oldValue;
    }

    public void Apply(TwoDAResourceEditorViewModel data)
    {
        data.Resource.SetCellText(RowID, ColumnHeader, NewValue);
        data.SelectedColumnIndex = data.Resource.GetColumnIndex(ColumnHeader);
        data.SelectedRowIndex = data.Resource.GetRowIndex(RowID);
    }

    public void Undo(TwoDAResourceEditorViewModel data)
    {
        data.Resource.SetCellText(RowID, ColumnHeader, OldValue);
        data.SelectedColumnIndex = data.Resource.GetColumnIndex(ColumnHeader);
        data.SelectedRowIndex = RowID;
    }
}
