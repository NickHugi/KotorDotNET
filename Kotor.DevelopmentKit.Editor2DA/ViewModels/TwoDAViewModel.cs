﻿using System;
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

    public ObservableCollection<ColumnViewModel> Columns { get; } = [];

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
        Columns.Clear();
        Columns.AddRange(["Row Header", .. twoda.GetColumns()]);

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

    public int GetRowID(int rowIndex)
    {
        var row = Rows.ElementAt(rowIndex);
        return _rowsSource.Items.IndexOf(row);
    }

    public void SetCellText(int rowID, string columnName, string value)
    {
        var columnIndex = Columns.ToList().FindIndex(x => x.Header == columnName);

        _rowsSource.Edit(rows =>
        {
            var row = rows[rowID].ToList();
            row[columnIndex] = value;
            rows.Replace(rows[rowID], row);
        });
    }

    public string GetCellText(int rowID, string columnName)
    {
        var columnIndex = GetColumnIndex(columnName);
        return _rowsSource.Items[rowID].ElementAt(columnIndex);
    }

    public void AddRow()
    {
        _rowsSource.Edit(rows =>
        {
            rows.Add([rows.Count.ToString(), .. Columns.Select(x => "")]);
        });
    }
    public void AddRow(int rowID)
    {
        _rowsSource.Edit(rows =>
        {
            rows.Insert(rowID, [rows.Count.ToString(), .. Columns.Select(x => "")]);
        });
    }

    public void RemoveRow()
    {
        _rowsSource.RemoveAt(_rowsSource.Count - 1);
    }
    public void RemoveRow(int rowID)
    {
        _rowsSource.RemoveAt(rowID);
    }

    public void AddColumn(string columnHeader)
    {
        AddColumn(columnHeader, _rowsSource.Items.Select(x => ""));
    }
    public void AddColumn(string columnHeader, IEnumerable<string> newCellValues)
    {
        Columns.Add(columnHeader);

        _rowsSource.Edit(rows =>
        {
            for (int i = 0; i < rows.Count; i++)
            {
                rows[i].Add(newCellValues.ElementAtOrDefault(i) ?? "");
            }
        });
    }
    public void AddColumn(string columnHeader, IEnumerable<string> newCellValues, int columnIndex)
    {
        Columns.Insert(columnIndex, columnHeader);

        _rowsSource.Edit(rows =>
        {
            for (int i = 0; i < rows.Count; i++)
            {
                rows[i].Add(newCellValues.ElementAtOrDefault(i) ?? "");
            }
        });
    }

    public void RemoveColumn(string columnHeader)
    {
        var columnIndex = GetColumnIndex(columnHeader);

        _rowsSource.Edit(rows =>
        {
            foreach (var row in rows)
            {
                row.RemoveAt(columnIndex);
            }
        });

        Columns.RemoveAt(columnIndex);
    }

    public int GetColumnIndex(string columnHeader)
    {
        return Columns.ToList().FindIndex(x => x.Header == columnHeader);
    }

    public void ResetRowHeaders()
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
