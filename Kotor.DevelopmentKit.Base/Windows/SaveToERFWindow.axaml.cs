using System;
using System.IO;
using System.Linq;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using DynamicData;
using Kotor.DevelopmentKit.Base.ViewModels;
using Kotor.NET.Common.Data;
using Kotor.NET.Encapsulations;
using Kotor.NET.Resources.KotorRIM;
using Kotor.NET.Tests.Encapsulation;

namespace Kotor.DevelopmentKit.Base.Windows;

public partial class SaveToERFWindow : Window
{
    private SaveToERFWindowViewModel _model => (SaveToERFWindowViewModel)DataContext!;

    public SaveToERFWindow()
    {
        InitializeComponent();
    }

    private void Cancel_Click(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        Close(null);
    }

    private void Save_Click(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        Close(_model.SelectedItem);
    }
}
