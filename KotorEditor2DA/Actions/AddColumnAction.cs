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
        data.Resource.AddColumn(ColumnHeader);
    }

    public void Undo(TwoDAResourceEditorViewModel data)
    {
        data.Resource.RemoveColumn(ColumnHeader);
    }
}
