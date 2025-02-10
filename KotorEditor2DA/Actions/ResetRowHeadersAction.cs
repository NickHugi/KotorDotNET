using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Kotor.DevelopmentKit.Base.Common;
using Kotor.DevelopmentKit.Editor2DA.ViewModels;

namespace Kotor.DevelopmentKit.Editor2DA.Actions;

public class ResetRowHeadersAction : IAction<TwoDAResourceEditorViewModel>
{
    private List<string> _oldRowHeaders;

    public ResetRowHeadersAction(IEnumerable<string> oldRowHeaders)
    {
        _oldRowHeaders = oldRowHeaders.ToList();
    }

    public void Apply(TwoDAResourceEditorViewModel data)
    {
        data.Resource.ResetRowHeaders();
    }

    public void Undo(TwoDAResourceEditorViewModel data)
    {
        for (int i = 0; i < _oldRowHeaders.Count; i++)
        {
            data.Resource.SetCellText(i, "Row Header", _oldRowHeaders[i]);
        }
    }
}
