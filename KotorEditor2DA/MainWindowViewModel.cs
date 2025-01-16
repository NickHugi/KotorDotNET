using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Reactive.Linq;
using System.Text;
using System.Threading.Tasks;
using DynamicData;
using Kotor.NET.Resources.Kotor2DA;
using Kotor.NET.Resources.Kotor2DA.Events;
using Microsoft.VisualBasic;
using ReactiveUI;
using Avalonia.Threading;
using Avalonia.ReactiveUI;
using DynamicData.Binding;
using Kotor.NET.Formats.Binary2DA.Serialisation;
using System.IO;
using Kotor.DevelopmentKit.Editor2DA;

namespace KotorEditor2DA;

public class MainWindowViewModel : TwoDAResourceEditorViewModel
{

}

public class __MainWindowViewModel : ReactiveObject
{
    private string _filter;
    public string Filter
    {
        get => _filter;
        set => this.RaiseAndSetIfChanged(ref _filter, value);
    }

    private string? _filepath;
    public string? FilePath
    {
        get => _filepath;
        set => this.RaiseAndSetIfChanged(ref _filepath, value);
    }
    public bool FilePathAssigned => FilePath is not null;

    private ObservableCollection<string> _columns;
    public ObservableCollection<string> Columns
    {
        get => _columns;
        set => this.RaiseAndSetIfChanged(ref _columns, value);
    }

    private SourceList<List<string>> _rowsSource;
    private readonly ReadOnlyObservableCollection<List<string>> _rows;
    public ReadOnlyObservableCollection<List<string>> Rows => _rows;


    public __MainWindowViewModel()
    {
        _filepath = null;
        this.WhenAnyValue(x => x.FilePath).Subscribe(x => this.RaisePropertyChanged(nameof(FilePathAssigned)));

        _filter = "";
        _columns = new();

        _rowsSource = new();
        var sorter = new ListIndexComperer<List<string>>(_rowsSource);
        _rowsSource.Connect()
            .ObserveOn(AvaloniaScheduler.Instance)
            .AutoRefreshOnObservable(x => this.ObservableForProperty(x => x.Filter))
            .Filter(row => row.Any(cell => cell.ToLower().Contains(Filter.ToLower())))
            .Sort(sorter)
            .Bind(out _rows)
            .Subscribe();

        NewFile();
    }

    public void LoadModel(TwoDA twoda)
    {
        Filter = "";

        Columns = ["Row Header", .. twoda.GetColumns()];

        _rowsSource.Clear();
        _rowsSource.AddRange(twoda.GetRows().Select<TwoDARow, List<string>>(x => [x.RowHeader, .. Columns.Skip(1).Select(y => x.GetCell(y).AsString())]).ToList());
    }
    public TwoDA BuildModel()
    {
        TwoDA twoda = new();
        var columns = Columns.Skip(1).ToList();

        columns.ToList().ForEach(columnHeader => twoda.AddColumn(columnHeader));

        Rows.ToList().ForEach(cells =>
        {
            var rowHeader = cells.First();
            var twodaRow = twoda.AddRow(rowHeader);

            for (int i = 0; i < cells.Count - 1; i++)
            {
                var columnHeader = columns.ElementAt(i);
                var value = cells.Skip(1).ElementAt(i);
                twodaRow.GetCell(columnHeader).SetString(value);
            }
        });

        return twoda;
    }

    public void NewFile()
    {
        FilePath = null;
        LoadModel(new());
    }
    public void LoadFromFile(string filepath)
    {
        FilePath = filepath;

        var twoda = TwoDA.FromFile(FilePath);
        LoadModel(twoda);
    }
    public void SaveToFile(string filepath)
    {
        FilePath = filepath;
        SaveToFile();
    }
    public void SaveToFile()
    {
        var twoda = BuildModel();
        using var fileStream = File.OpenWrite(FilePath);
        new TwoDABinarySerializer(twoda).Serialize().Write(fileStream);
    }

    public void NewRow()
    {

    }
    public void DeleteRow(int rowIndex)
    {

    }
    public void ApplyFilter(string filter)
    {

    }
}

public class ListIndexComperer<T> : IComparer<T> where T : notnull
{
    private SourceList<T> _sauce;

    public ListIndexComperer(SourceList<T> sauce)
    {
        _sauce = sauce;
    }

    public int Compare(T? x, T? y)
    {
        var a = _sauce.Items.IndexOf(x);
        var b = _sauce.Items.IndexOf(y);
        return a - b;
    }
}
