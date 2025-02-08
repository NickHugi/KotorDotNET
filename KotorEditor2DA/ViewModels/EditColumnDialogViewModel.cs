using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ReactiveUI;
using ReactiveUI.Validation.Extensions;
using ReactiveUI.Validation.Helpers;

namespace Kotor.DevelopmentKit.Editor2DA.ViewModels;

public partial class EditColumnDialogViewModel : ReactiveValidationObject
{
    private string _columnHeader = "";
    public string ColumnHeader
    {
        get => _columnHeader;
        set => this.RaiseAndSetIfChanged(ref _columnHeader, value);
    }

    private IEnumerable<string> _reservedColumnHeaders = default!;
    public IEnumerable<string> ReservedColumnHeaders
    {
        get => _reservedColumnHeaders;
        set => this.RaiseAndSetIfChanged(ref _reservedColumnHeaders, value);
    }

    public EditColumnDialogViewModel(IEnumerable<string> reservedColumnHeaders)
    {
        ReservedColumnHeaders = reservedColumnHeaders;
        
        this.ValidationRule(
            viewModel => viewModel.ColumnHeader,
            columnHeader => !ReservedColumnHeaders.Contains(columnHeader),
            "The specified column header already is in use.");
    }
}
