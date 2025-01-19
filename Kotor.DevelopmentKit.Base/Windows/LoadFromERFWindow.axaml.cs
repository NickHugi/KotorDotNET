using System;
using System.IO;
using System.Linq;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using DynamicData;
using Kotor.DevelopmentKit.Base.ViewModels;
using Kotor.NET.Common.Data;
using Kotor.NET.Resources.KotorRIM;
using Kotor.NET.Tests.Encapsulation;

namespace Kotor.DevelopmentKit.Base;

public partial class LoadFromERFWindow : Window
{
    private LoadFromERFWindowViewModel _model => (LoadFromERFWindowViewModel)DataContext!;

    public LoadFromERFWindow(string filepath, string defaultResRefFilter = "", ResourceType[] defaultTypeFilter = null)
    {
        DataContext = new LoadFromERFWindowViewModel()
        {
            ResRefFilter = defaultResRefFilter,
            TypeFilter = defaultTypeFilter,
        };

        InitializeComponent();
        LoadEncapsulatedData(filepath);
    }

    public void LoadEncapsulatedData(string filepath)
    {
        var extension = Path.GetExtension(filepath).ToLower();
        IEncapsulatedFormat encapsulator = extension switch
        {
            ".erf" => new ERFEncapsulation(filepath),
            ".mod" => new ERFEncapsulation(filepath),
            ".rim" => new RIMEncapsulation(filepath),
            _ => throw new ArgumentException("The filepath loaded was not a valid RIM/ERF file.")
        };
        _model.LoadModel(encapsulator);
    }

    private void Button_Click(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        Close(_model.SelectedItem);
    }

    private void Load_Click(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        Close("abc");
    }
}
