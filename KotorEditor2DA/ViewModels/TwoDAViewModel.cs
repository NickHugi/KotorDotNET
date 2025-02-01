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

    private string _filter = "";
    public string Filter
    {
        get => _filter;
        set => this.RaiseAndSetIfChanged(ref _filter, value);
    }

    private ObservableCollection<string> _columns = [];
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
        var sorter = new SourceListIndexComperer<List<string>>(_rowsSource);
        _rowsSource.Connect()
            .ObserveOn(AvaloniaScheduler.Instance)
            .AutoRefreshOnObservable(x => this.ObservableForProperty(x => x.Filter))
            .Filter(row => row.Any(cell => cell.ToLower().Contains(Filter.ToLower())))
            .Sort(sorter)
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


    public void SetCellText(int rowIndex, string columnName, string value)
    {
        var columnIndex = Columns.IndexOf(columnName);

        _rowsSource.Edit(rows =>
        {
            var row = rows[rowIndex].ToList();
            row[columnIndex] = value;
            rows.Replace(rows[rowIndex], row);
        });
    }

    public void AddRow()
    {
        _rowsSource.Edit(rows =>
        {
            rows.Add([rows.Count.ToString(), .. Columns.Select(x => "")]);
        });
    }

    public void ResetRowLabels()
    {
        _rowsSource.Edit(rows =>
        {
            for (int i = 0; i < rows.Count; i++)
            {
                var row = rows[i].ToList();
                row[0] = i.ToString();
                rows.Replace(rows[i], row);
            }
        });
    }
}
