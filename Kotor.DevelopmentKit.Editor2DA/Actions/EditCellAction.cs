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
    public int RowIndex { get; }
    public string ColumnHeader { get; }
    public string NewValue { get; }
    public string OldValue { get; }

    public EditCellAction(int rowIndex, string columnHeader, string newValue, string oldValue)
    {
        RowIndex = rowIndex;
        ColumnHeader = columnHeader;
        NewValue = newValue;
        OldValue = oldValue;
    }

    public void Apply(TwoDAResourceEditorViewModel data)
    {
        data.Resource.SetCellText(RowIndex, ColumnHeader, NewValue);
        data.SelectedColumnIndex = data.Resource.Columns.IndexOf(ColumnHeader);
        data.SelectedRowIndex = RowIndex;
    }

    public void Undo(TwoDAResourceEditorViewModel data)
    {
        data.Resource.SetCellText(RowIndex, ColumnHeader, OldValue);
        data.SelectedColumnIndex = data.Resource.Columns.IndexOf(ColumnHeader);
        data.SelectedRowIndex = RowIndex;
    }
}
