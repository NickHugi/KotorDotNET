using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Kotor.DevelopmentKit.Base.ViewModels;

namespace Kotor.DevelopmentKit.Base.Controls;

public partial class ERFResourceList : UserControl
{
    public ERFResourceListViewModel Context => (ERFResourceListViewModel)DataContext!;

    public ERFResourceList()
    {
        InitializeComponent();
    }
}
