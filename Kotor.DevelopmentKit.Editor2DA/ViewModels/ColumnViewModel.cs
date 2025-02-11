using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ReactiveUI;

namespace Kotor.DevelopmentKit.Editor2DA.ViewModels;

public class ColumnViewModel : ReactiveObject
{
    private string _header;
    public string Header
    {
        get => _header;
        set => this.RaiseAndSetIfChanged(ref _header, value);
    }

    public override string ToString() => Header;
    public static implicit operator ColumnViewModel(string header) => new() { Header = header };
    public static implicit operator string(ColumnViewModel column) => column.Header;
}
