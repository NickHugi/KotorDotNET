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
using Kotor.DevelopmentKit.Base.Windows;
using Kotor.DevelopmentKit.Editor2DA;
using Kotor.DevelopmentKit.Editor2DA.ViewModels;
using Kotor.NET.Common.Data;
using Kotor.NET.Encapsulations;
using Kotor.NET.Formats.Binary2DA.Serialisation;
using Kotor.NET.Resources.Kotor2DA;
using Tmds.DBus.Protocol;

namespace Kotor.DevelopmentKit.Editor2DA;

public partial class TwoDAResourceEditor : ResourceEditorBase
{
    public TwoDAResourceEditorViewModel Context => (TwoDAResourceEditorViewModel)DataContext!;

    public override FilePickerOpenOptions FilePickerOpenOptions => new()
    {
        Title = "Open 2DA File",
        AllowMultiple = false,
        FileTypeFilter = [FilePickerTypes.TwoDA, FilePickerTypes.Encapsulator],
    };
    public override FilePickerSaveOptions FilePickerSaveOptions => new()
    {
        Title = "Save 2DA File",
        ShowOverwritePrompt = false,
        FileTypeChoices = [FilePickerTypes.TwoDA, FilePickerTypes.Encapsulator],
    };
    public override List<ResourceType> ResourceTypes => [ResourceType.TWODA];


    public TwoDAResourceEditor()
    {
        InitializeComponent();
    }


    protected override void LoadFromFile()
    {
        Context.LoadFromFile();
    }
    protected override void LoadFromFile(string filepath)
    {
        Context.LoadFromFile(filepath);
    }
    protected override void LoadFromFile(string filepath, ResRef resref, ResourceType resourceType)
    {
        Context.LoadFromFile(filepath, resref, resourceType);
    }

    protected override void SaveToFile()
    {
        Context.SaveToFile();
    }
    protected override void SaveToFile(string filepath)
    {
        Context.SaveToFile(filepath);
    }
    protected override void SaveToFile(string filepath, ResRef resref, ResourceType resourceType)
    {
        Context.SaveToFile(filepath, resref, resourceType);
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


    private void MenuItem_New_Click(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        Context.NewFile();
    }

    private void MenuItem_Open_Click(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        OpenFileDialog();
    }

    private void MenuItem_Save_Click(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        Context.SaveToFile();
    }

    private void MenuItem_SaveAs_Click(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        SaveFileDialog();
    }

    private void MenuItem_Reset_Click(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        Context.LoadFromFile();
    }
}
