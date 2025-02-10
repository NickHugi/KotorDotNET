using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;
using System.Threading.Tasks;
using Avalonia.Controls;
using Avalonia.Data;
using Avalonia.Input;
using Avalonia.Platform.Storage;
using Avalonia.Threading;
using Kotor.DevelopmentKit.Base.Common;
using Kotor.DevelopmentKit.Base.Windows;
using Kotor.DevelopmentKit.Editor2DA.ViewModels;
using Kotor.DevelopmentKit.Editor2DA.Windows;
using Kotor.NET.Common.Data;
using ReactiveUI;

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

    /// <summary>
    /// Track the value of the selected cell before editing has finished.
    /// </summary>
    private string _originalCellValue = "";


    public TwoDAResourceEditor()
    {
        InitializeComponent();
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

        var rowIndex = Context.SelectedRowIndex;
        var rowID = Context.Resource.GetRowID(Context.SelectedRowIndex);
        var currentColumn = TwodaDataGrid.CurrentColumn;
        var columnHeader = (string)TwodaDataGrid.CurrentColumn.Header;
        var newValue = await Clipboard.GetTextAsync() ?? "";

        Context.EditCell(rowID, columnHeader, newValue);
        TwodaDataGrid.CurrentColumn = currentColumn;
        TwodaDataGrid.SelectedIndex = rowIndex;
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
        Context.ResetRowHeaders();
    }

    public void AddRow()
    {
        Context.AddRow();
    }

    public async void AddColumn()
    {
        var dialog = new EditColumnDialog()
        {
            Title = "Create new column",
            DataContext = new EditColumnDialogViewModel(Context.Resource.Columns.ToArray())
        };
        var columnHeader = await dialog.ShowDialog<string>(this);
        Context.AddColumn(columnHeader);
    }

    public void Undo()
    {
        // For whatever reason calling Undo/Redo directly from a MenuItem command causes the
        // DataGrid to display the wrong data on the affected row.
        Dispatcher.UIThread.Post(() => Context.Undo(), DispatcherPriority.Default);
    }

    public void Redo()
    {
        Dispatcher.UIThread.Post(() => Context.Redo(), DispatcherPriority.Default);
    }


    protected override void OnDataContextChanged(EventArgs e)
    {
        base.OnDataContextChanged(e);

        Context.WhenAnyValue(x => x.SelectedColumnIndex).Subscribe(x =>
        {
            var column = TwodaDataGrid.Columns.SingleOrDefault(x => x.DisplayIndex == Context.SelectedColumnIndex);
            if (column is not null)
                TwodaDataGrid.CurrentColumn = column;
        });

        Context.Resource.WhenAnyValue(x => x.Columns.Count).Subscribe(x =>
        {
            RefreshColumns();
        });
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

        Context.SelectedColumnIndex = TwodaDataGrid.CurrentColumn?.DisplayIndex ?? 0;
    }

    private void DataGrid_BeginningEdit(object? sender, Avalonia.Controls.DataGridBeginningEditEventArgs e)
    {
        _originalCellValue = ((IEnumerable<string>)e.Row.DataContext!).ElementAt(e.Column.DisplayIndex);
    }

    private void DataGrid_CellEditEnded(object? sender, Avalonia.Controls.DataGridCellEditEndedEventArgs e)
    {
        var rowID = Context.Resource.GetRowID(Context.SelectedRowIndex);
        var newValue = ((IEnumerable<string>)e.Row.DataContext!).ElementAt(e.Column.DisplayIndex);
        var columnHeader = (string)e.Column.Header;
        Context.EditCell(rowID, columnHeader, newValue, _originalCellValue);
    }

    private void DataGrid_PointerReleased(object? sender, Avalonia.Input.PointerReleasedEventArgs e)
    {
        Context.SelectedColumnIndex = TwodaDataGrid.CurrentColumn?.DisplayIndex ?? 0;
    }
}
