using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data.Common;
using System.Globalization;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Threading.Tasks;
using Avalonia.Controls;
using Avalonia.Controls.Primitives;
using Avalonia.Controls.Templates;
using Avalonia.Data;
using Avalonia.Data.Converters;
using Avalonia.Input;
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
        TwodaDataGrid.Columns.Clear();

        for (int i = 0; i < Context.Resource.Columns.Count(); i++)
        {
            var column = Context.Resource.Columns.ElementAt(i);

            TwodaDataGrid.Columns.Add(new DataGridTextColumn()
            {
                Header = column,
                Binding = new Binding($"[{i}]"),
                IsReadOnly = false,
            });
        }
    }

    public async Task CopySelectedCell()
    {
        if (Clipboard is null)
            return;

        var rowIndex = TwodaDataGrid.SelectedIndex;
        var columnHeader = (string)TwodaDataGrid.CurrentColumn.Header;
        var columnIndex = Context.Resource.Columns.IndexOf(columnHeader);
        var text = Context.Resource.Rows[rowIndex][columnIndex];
        await Clipboard.SetTextAsync(text);
    }

    public async Task PasteSelectedCell()
    {
        if (Clipboard is null)
            return;

        var text = await Clipboard.GetTextAsync() ?? "";
        var rowIndex = TwodaDataGrid.SelectedIndex;
        var columnHeader = (string)TwodaDataGrid.CurrentColumn.Header;
        Context.Resource.SetCellText(rowIndex, columnHeader, text);
    }

    public void NewFile()
    {
        Context.NewFile();
    }

    public async Task OpenFile()
    {
        var resource = await SaveResourcePicker();

        if (resource is not null)
        {
            Context.LoadFromFile(resource.FilePath, resource.ResRef, resource.ResourceType);
        }
    }

    public void SaveFile()
    {
        Context.SaveToFile();
    }

    public async Task SaveFileAs()
    {
        var resource = await SaveResourcePicker();

        if (resource is not null)
        {
            Context.SaveToFile(resource.FilePath, resource.ResRef, resource.ResourceType);
        }
    }

    public void ResetFile()
    {
        Context.LoadFromFile();
    }

    public void ToggleFilter()
    {
        Context.ToggleFilter();
    }

    public void ResetSorting()
    {
        TwodaDataGrid.Columns.ToList().ForEach(x => x.ClearSort());
    }

    public void ResetRowHeaders()
    {
        Context.Resource.ResetRowLabels();
    }

    public void AddRow()
    {
        Context.Resource.AddRow();
    }

    private void Window_Loaded(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        if (Design.IsDesignMode)
            return;

        RefreshColumns();
        Context.Resource.PropertyChanged += (sender, args) =>
        {
            if (args.PropertyName == nameof(Context.Resource.Columns))
                RefreshColumns();
        };
    }

    private async void DataGrid_KeyUp(object? sender, Avalonia.Input.KeyEventArgs e)
    {
        if (e.KeyModifiers == KeyModifiers.Control)
        {
            if (e.Key == Key.C)
            {
                await CopySelectedCell();
            }
            else if (e.Key == Key.V)
            {
                await PasteSelectedCell();
            }
        }
    }
}
