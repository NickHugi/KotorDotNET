using System;
using System.IO;
using System.Linq;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using DynamicData;
using Kotor.DevelopmentKit.Base.DialogResults;
using Kotor.DevelopmentKit.Base.ViewModels;
using Kotor.NET.Common.Data;
using Kotor.NET.Encapsulations;
using Kotor.NET.Resources.KotorRIM;
using Kotor.NET.Tests.Encapsulation;

namespace Kotor.DevelopmentKit.Base.Windows;

public partial class LoadFromERFWindow : Window
{
    public LoadFromERFWindowViewModel Context => (LoadFromERFWindowViewModel)DataContext!;

    public LoadFromERFWindow()
    {
        InitializeComponent();
    }

    private void Cancel_Click(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        Close(null);
    }

    private void Load_Click(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        Close(new LoadFromERFWindowDialogResult
        {
            FilePath = Context.ResourceList.SelectedItem.Filepath,
            ResRef = Context.ResourceList.SelectedItem.ResRef,
            ResourceType = Context.ResourceList.SelectedItem.Type,
        });
    }
}
