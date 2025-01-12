using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.Common;
using System.Globalization;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using Avalonia.Controls;
using Avalonia.Controls.Primitives;
using Avalonia.Controls.Templates;
using Avalonia.Data;
using Avalonia.Data.Converters;
using Avalonia.Markup.Xaml.Templates;
using Kotor.NET.Resources.Kotor2DA;
using Tmds.DBus.Protocol;

namespace KotorEditor2DA;

public partial class MainWindow : Window
{
    public MainWindowViewModel Model => (MainWindowViewModel)DataContext!;


    public MainWindow()
    {
        var model = new MainWindowViewModel();
        DataContext = model;
        InitializeComponent();

        RefreshColumns(model);
        model.PropertyChanged += (sender, args) =>
        {
            if (args.PropertyName == nameof(Model.Columns))
                RefreshColumns(model);
        };
    }

    private void RefreshColumns(MainWindowViewModel model)
    {
        Griddy.Columns.Clear();

        Griddy.Columns.Add(new DataGridTemplateColumn()
        {
            CellTemplate = new FuncDataTemplate<object>((value, namescope) =>
            {
                return new TextBlock
                {
                    [!TextBlock.TextProperty] = new Binding
                    {
                        Path = "Index",
                        RelativeSource = new RelativeSource(RelativeSourceMode.FindAncestor) {  AncestorType = typeof(DataGridRow)},
                    }
                };
            })
        });

        for (int i = 0; i < model.Columns.Count(); i++)
        {
            var column = model.Columns.ElementAt(i);

            Griddy.Columns.Add(new DataGridTextColumn()
            {
                Header = column,
                Binding = new Binding($"[{i}]"),
                IsReadOnly = false,
            });
        }
    }

    private void DataGrid_CellEditEnded(object? sender, Avalonia.Controls.DataGridCellEditEndedEventArgs e)
    {
        var value = ((ObservableCollection<string>)e.Row.DataContext!).ElementAt(e.Column.DisplayIndex - 1);
        Model.EditCell((string)e.Column.Header, e.Row.Index, value);
    }

    private async void MenuItem_Click(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        var files = await TopLevel.GetTopLevel(this).StorageProvider.OpenFilePickerAsync(new()
        {
            Title = "Open 2DA File",
            AllowMultiple = false,
            // TODO FileTypeFilter 
        });

        if (files.Count ==  1)
        {
            var twoda = TwoDA.FromFile(files[0].Path.AbsolutePath);
            Model.ReadModel(twoda);
        }
    }
}
