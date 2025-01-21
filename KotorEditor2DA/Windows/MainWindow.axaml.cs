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
using Kotor.DevelopmentKit.Base;
using Kotor.DevelopmentKit.Base.Common;
using Kotor.DevelopmentKit.Base.ViewModels;
using Kotor.DevelopmentKit.Editor2DA;
using Kotor.DevelopmentKit.Editor2DA.ViewModels;
using Kotor.NET.Common.Data;
using Kotor.NET.Encapsulations;
using Kotor.NET.Formats.Binary2DA.Serialisation;
using Kotor.NET.Resources.Kotor2DA;
using Tmds.DBus.Protocol;

namespace Kotor.DevelopmentKit.Editor2DA;

public partial class MainWindow : Window
{
    public TwoDAResourceEditorViewModel Context => (TwoDAResourceEditorViewModel)DataContext!;

    public MainWindow()
    {
        InitializeComponent();
    }

    private void RefreshColumns()
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

        for (int i = 0; i < Context.Resource.Columns.Count(); i++)
        {
            var column = Context.Resource.Columns.ElementAt(i);

            Griddy.Columns.Add(new DataGridTextColumn()
            {
                Header = column,
                Binding = new Binding($"[{i}]"),
                IsReadOnly = false,
            });
        }
    }
    
    private void Window_Loaded(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        RefreshColumns();
        Context.Resource.PropertyChanged += (sender, args) =>
        {
            if (args.PropertyName == nameof(Context.Resource.Columns))
                RefreshColumns();
        };
    }

    private async void MenuItem_New_Click(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        Context.NewFile();
    }

    private async void MenuItem_Open_Click(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        var files = await TopLevel.GetTopLevel(this).StorageProvider.OpenFilePickerAsync(new()
        {
            Title = "Open 2DA File",
            AllowMultiple = false,
            FileTypeFilter = [FilePickerTypes.TwoDA, FilePickerTypes.Encapsulator],
        });

        var file = files.FirstOrDefault();

        if (file is null)
        {

        }
        else if (file.Path.AbsolutePath.EndsWith(".rim"))
        {
            var encapsulatorPicker = new LoadFromERFWindow()
            {
                DataContext = new LoadFromERFWindowViewModel(Encapsulation.LoadFromPath(file.Path.LocalPath))
                {
                    ResourceTypeFilter = [ResourceType.TWODA]
                }
            };
            var resource = await encapsulatorPicker.ShowDialog<ResourceViewModel>(this);
            Context.LoadFromFile(resource.Filepath, resource.ResRef, resource.Type);
        }
        else if (files.Count ==  1)
        {
            Context.LoadFromFile(files[0].Path.AbsolutePath);
        }
    }

    private async void MenuItem_Save_Click(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        Context.SaveToFile();
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
            Context.SaveToFile(file.Path.AbsolutePath);
        }
    }
}
