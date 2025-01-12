using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Kotor.NET.Resources.Kotor2DA;
using Kotor.NET.Resources.Kotor2DA.Events;
using Microsoft.VisualBasic;

namespace KotorEditor2DA;

public class MainWindowViewModel : INotifyPropertyChanged
{
    private string _filter;
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

    public ObservableCollection<string> Columns { get; set; } = new();
    public ObservableCollection<ObservableCollection<string>> Rows { get; set; } = new();

    public MainWindowViewModel()
    {
        //ReadModel(new TwoDA());
        _filter = "";
    }

    public event PropertyChangedEventHandler? PropertyChanged = delegate { };

    public void ReadModel(TwoDA twoda)
    {
        Columns = ["Row Header", .. twoda.GetColumns()];
        Rows = new ObservableCollection<ObservableCollection<string>>(twoda.GetRows().Select<TwoDARow, ObservableCollection<string>>(x => [x.RowHeader, .. Columns.Select(y => x.GetCell(y).AsString())]).ToList());

        PropertyChanged(this, new(nameof(Columns)));
        PropertyChanged(this, new(nameof(Rows)));
    }
    public TwoDA WriteModel()
    {
        return null;
    }

    public void NewRow()
    {

    }
    public void DeleteRow(int rowIndex)
    {

    }
    public void EditCell(string columnName, int rowIndex, string value)
    {
        var columnIndex = Columns.IndexOf(columnName);
        Rows[rowIndex][columnIndex] = value;
    }
    public void ApplyFilter(string filter)
    {

    }
}
