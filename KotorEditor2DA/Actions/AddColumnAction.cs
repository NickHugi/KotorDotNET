using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Kotor.DevelopmentKit.Base.Common;
using Kotor.DevelopmentKit.Editor2DA.ViewModels;

namespace Kotor.DevelopmentKit.Editor2DA.Actions;

public class AddColumnAction : IAction<TwoDAResourceEditorViewModel>
{
    public string ColumnHeader { get; }

    public AddColumnAction(string columnHeader)
    {
        ColumnHeader = columnHeader;
    }

    public void Apply(TwoDAResourceEditorViewModel data)
    {
        data.Resource.AddRow();
        data.SelectedRowIndex = data.Resource.Rows.Count - 1;
    }

    public void Undo(TwoDAResourceEditorViewModel data)
    {
        data.Resource.RemoveRow(data.Resource.Rows.Count - 1);
        data.SelectedRowIndex = data.Resource.Rows.Count - 1;
    }
}
