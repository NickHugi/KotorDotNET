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
using Avalonia.Platform.Storage;
using Kotor.DevelopmentKit.Base.Common;
using Kotor.NET.Formats.Binary2DA.Serialisation;
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
        model.Resource.PropertyChanged += (sender, args) =>
        {
            if (args.PropertyName == nameof(Model.Resource.Columns))
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
                    //[!TextBlock.TextProperty] = new Binding
                    //{
                    //    Path = "Index",
                    //    RelativeSource = new RelativeSource(RelativeSourceMode.FindAncestor) {  AncestorType = typeof(DataGridRow)},
                    //}
                };
            }),
            Width = DataGridLength.SizeToCells
        });

        for (int i = 0; i < model.Resource.Columns.Count(); i++)
        {
            var column = model.Resource.Columns.ElementAt(i);

            Griddy.Columns.Add(new DataGridTextColumn()
            {
                Header = column,
                Binding = new Binding($"[{i}]"),
                IsReadOnly = false,
            });
        }
    }

    #region Events
    private async void MenuItem_New_Click(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        Model.NewFile();
    }

    private async void MenuItem_Open_Click(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        var files = await TopLevel.GetTopLevel(this).StorageProvider.OpenFilePickerAsync(new()
        {
            Title = "Open 2DA File",
            AllowMultiple = false,
            FileTypeFilter = [FilePickerTypes.TwoDA],
        });

        if (files.Count ==  1)
        {
            Model.LoadFromFile(files[0].Path.AbsolutePath);
        }
    }

    private async void MenuItem_Save_Click(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        Model.SaveToFile();
    }

    private async void MenuItem_SaveAs_Click(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        var file = await TopLevel.GetTopLevel(this).StorageProvider.SaveFilePickerAsync(new()
        {
            Title = "Save 2DA File",
            ShowOverwritePrompt = true,
            FileTypeChoices = [FilePickerTypes.TwoDA],
        });

        if (file is not null)
        {
            Model.SaveToFile(file.Path.AbsolutePath);
        }
    }
    #endregion
}
