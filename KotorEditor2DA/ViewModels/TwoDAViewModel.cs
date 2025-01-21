using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reactive.Linq;
using System.Text;
using System.Threading.Tasks;
using Avalonia.ReactiveUI;
using DynamicData;
using Kotor.DevelopmentKit.Base;
using Kotor.NET.Resources.Kotor2DA;
using ReactiveUI;

namespace Kotor.DevelopmentKit.Editor2DA.ViewModels;

public class TwoDAViewModel : ReactiveObject
{

    private string _filter;
    public string Filter
    {
        get => _filter;
        set => this.RaiseAndSetIfChanged(ref _filter, value);
    }

    private ObservableCollection<string> _columns;
    public ObservableCollection<string> Columns
    {
        get => _columns;
        set => this.RaiseAndSetIfChanged(ref _columns, value);
    }

    private SourceList<List<string>> _rowsSource;
    private readonly ReadOnlyObservableCollection<List<string>> _rows;
    public ReadOnlyObservableCollection<List<string>> Rows => _rows;

    public TwoDAViewModel()
    {
        _rowsSource = new();
        _rowsSource.Connect()
            .ObserveOn(AvaloniaScheduler.Instance)
            .AutoRefreshOnObservable(x => this.ObservableForProperty(x => x.Filter))
            .Filter(row => row.Any(cell => cell.ToLower().Contains(Filter.ToLower())))
            .Sort(new SourceListIndexComperer<List<string>>(_rowsSource))
            .Bind(out _rows)
            .Subscribe();

        Load(new());
    }


    public void Load(TwoDA twoda)
    {
        Columns = ["Row Header", .. twoda.GetColumns()];

        _rowsSource.Clear();
        _rowsSource.AddRange(twoda.GetRows().Select<TwoDARow, List<string>>(x => [x.RowHeader, .. Columns.Skip(1).Select(y => x.GetCell(y).AsString())]).ToList());
    }

    public TwoDA Build()
    {
        TwoDA twoda = new();
        var columns = Columns.Skip(1).ToList();

        columns.ToList().ForEach(columnHeader => twoda.AddColumn(columnHeader));

        _rowsSource.Items.ToList().ForEach(cells =>
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
}
