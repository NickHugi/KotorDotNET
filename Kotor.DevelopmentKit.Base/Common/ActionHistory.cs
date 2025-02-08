using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kotor.DevelopmentKit.Base.Common;

public class ActionHistory<T> : INotifyPropertyChanged where T : class 
{
    private readonly T _state;
    private readonly Stack<IAction<T>> _undo = [];
    private readonly Stack<IAction<T>> _redo = [];

    public event PropertyChangedEventHandler? PropertyChanged = delegate { };

    public bool CanUndo => _undo.Any();
    public bool CanRedo => _redo.Any();

    public ActionHistory(T state)
    {
        _state = state;
    }

    public void Redo()
    {
        if (!CanRedo)
            return;

        var action = _redo.Pop();
        action.Apply(_state);

        _undo.Push(action);

        PropertyChanged!(this, new(nameof(CanUndo)));
    }

    public void Undo()
    {
        if (!CanUndo)
            return;

        var action = _undo.Pop();
        action.Undo(_state);

        _redo.Push(action);

        PropertyChanged!(this, new(nameof(CanRedo)));
    }

    public void Apply(IAction<T> action)
    {
        _redo.Clear();
        _undo.Push(action);

        action.Apply(_state);

        PropertyChanged!(this, new(nameof(CanUndo)));
        PropertyChanged!(this, new(nameof(CanRedo)));
    }

    public void Clear()
    {
        _redo.Clear();
        _undo.Clear();
    }
}
