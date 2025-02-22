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
    public int ColumnIndex { get; }

    public RemoveColumnAction(string columnHeader, int columnIndex, IEnumerable<string> cellValues)
    {
        ColumnHeader = columnHeader;
        ColumnIndex = columnIndex;
        CellValues = cellValues.ToList();
    }

    public void Apply(TwoDAResourceEditorViewModel data)
    {
        data.Resource.RemoveColumn(ColumnHeader);
    }

    public void Undo(TwoDAResourceEditorViewModel data)
    {
        data.Resource.AddColumn(ColumnHeader, CellValues, ColumnIndex);
    }
}
