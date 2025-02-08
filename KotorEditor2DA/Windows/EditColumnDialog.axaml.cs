using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Kotor.DevelopmentKit.Editor2DA.ViewModels;

namespace Kotor.DevelopmentKit.Editor2DA.Windows;

public partial class EditColumnDialog : Window
{
    public EditColumnDialogViewModel Context => (EditColumnDialogViewModel)DataContext!;

    public EditColumnDialog()
    {
        InitializeComponent();
    }

    public void Submit()
    {
        Close(Context.ColumnHeader);
    }

    public void Cancel()
    {
        Close(null);
    }
}
