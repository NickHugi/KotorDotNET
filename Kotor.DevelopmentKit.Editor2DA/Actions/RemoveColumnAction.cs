using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Kotor.DevelopmentKit.Base.Common;
using Kotor.DevelopmentKit.Editor2DA.ViewModels;

namespace Kotor.DevelopmentKit.Editor2DA.Actions;

public class RemoveColumnAction : IAction<TwoDAResourceEditorViewModel>
{
    public string ColumnHeader { get; }
    public List<string> CellValues { get; }

    private int? _deletedColumnIndex;

    public RemoveColumnAction(string columnHeader, IEnumerable<string> cellValues)
    {
        ColumnHeader = columnHeader;
        CellValues = cellValues.ToList();
    }

    public void Apply(TwoDAResourceEditorViewModel data)
    {
        _deletedColumnIndex = data.Resource.Columns.ToList().FindIndex(x => x.Header == ColumnHeader);
        data.Resource.RemoveColumn(ColumnHeader);
    }

    public void Undo(TwoDAResourceEditorViewModel data)
    {
        data.Resource.AddColumn(ColumnHeader, CellValues, _deletedColumnIndex.Value);

        //for (int i = 0; i < CellValues.Count; i++)
        //{
        //    data.Resource.SetCellText(i, ColumnHeader, CellValues[i]);
        //}
    }
}
