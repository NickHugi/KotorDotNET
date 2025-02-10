using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Kotor.DevelopmentKit.Base.Common;
using Kotor.DevelopmentKit.Editor2DA.ViewModels;

namespace Kotor.DevelopmentKit.Editor2DA.Actions;

public class RenameColumnAction : IAction<TwoDAResourceEditorViewModel>
{
    public string OldColumnHeader { get; }
    public string NewColumnHeader { get; }

    public RenameColumnAction(string oldColumnHeader, string newColumnHeader)
    {
        OldColumnHeader = oldColumnHeader;
        NewColumnHeader = newColumnHeader;
    }

    public void Apply(TwoDAResourceEditorViewModel data)
    {
    }

    public void Undo(TwoDAResourceEditorViewModel data)
    {
    }
}
