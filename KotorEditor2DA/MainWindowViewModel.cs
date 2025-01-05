using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Kotor.NET.Resources.Kotor2DA;
using Microsoft.VisualBasic;

namespace KotorEditor2DA;

public class MainWindowViewModel : INotifyPropertyChanged
{
    private TwoDA _twoda;
    private string _filter;

    //public IEnumerable<IEnumerable<string>> Rows => _twoda.GetRows().Select(x => Columns.Select(y => x.GetCell(y).AsString()).ToList()).Where(x => x.Any(y => y.Contains(_filter))).ToList();
    public IEnumerable<IEnumerable<string>> Rows => _twoda.GetRows().Select<TwoDARow, List<string>>(x => [x.RowHeader, .. Columns.Select(y => x.GetCell(y).AsString())]).ToList();

    public IEnumerable<string> Columns => ["Row Header", .. _twoda.GetColumns()];
    //public IEnumerable<string> Columns => _twoda.GetColumns();
    public string Filter
    {
        get => _filter;
        set
        {
            _filter = value;
            PropertyChanged(this, new(nameof(Filter)));
            PropertyChanged(this, new(nameof(Rows)));
        }
    } // TODO

    public MainWindowViewModel()
    {
        _twoda = new TwoDA();
        _filter = "";
        _twoda.RowChanged += (sender, args) => PropertyChanged(sender, new(nameof(Rows)));
        _twoda.ColumnChanged += (sender, args) => PropertyChanged(sender, new(nameof(Columns)));
    }

    public event PropertyChangedEventHandler? PropertyChanged = delegate { };


    public void Load2DA(TwoDA twoda)
    {
        _twoda = twoda;
        PropertyChanged(this, new(nameof(Rows)));
        PropertyChanged(this, new(nameof(Columns)));
    }
    public void NewRow()
    {

    }
    public void DeleteRow(int rowIndex)
    {

    }
    public void EditCell(string columnName, int rowIndex, string value)
    {
        _twoda.GetRow(rowIndex).GetCell(columnName).SetString(value);
    }
    public void ApplyFilter(string filter)
    {

    }
}
