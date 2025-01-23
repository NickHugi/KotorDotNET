using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Avalonia.Controls;
using Avalonia.Platform.Storage;
using Kotor.DevelopmentKit.Base.Common;
using Kotor.DevelopmentKit.Base.DialogResults;
using Kotor.DevelopmentKit.Base.ViewModels;
using Kotor.NET.Common.Data;
using Kotor.NET.Encapsulations;

namespace Kotor.DevelopmentKit.Base.Windows;

public abstract class ResourceEditorBase : Window
{
    public abstract FilePickerOpenOptions FilePickerOpenOptions { get; }
    public abstract FilePickerSaveOptions FilePickerSaveOptions { get; }
    public abstract List<ResourceType> ResourceTypes { get; }

    protected async void OpenFileDialog()
    {
        var files = await TopLevel.GetTopLevel(this)!.StorageProvider.OpenFilePickerAsync(FilePickerOpenOptions);

        var file = files.FirstOrDefault();

        if (file is null)
        {

        }
        else if (Encapsulation.IsPathEncapsulation(file.Path.LocalPath))
        {
            var encapsulatorPicker = new LoadFromERFWindow();
            encapsulatorPicker.DataContext = new LoadFromERFWindowViewModel().LoadModel(file.Path.LocalPath, ResourceTypes);

            var resource = await encapsulatorPicker.ShowDialog<LoadFromERFWindowDialogResult>(this);
            if (resource is not null)
            {
                LoadFromFile(file.Path.LocalPath, resource.ResRef, resource.ResourceType);
            }
        }
        else
        {
            LoadFromFile(file.Path.LocalPath);
        }
    }

    protected async void SaveFileDialog()
    {
        var file = await TopLevel.GetTopLevel(this)!.StorageProvider.SaveFilePickerAsync(FilePickerSaveOptions);

        if (file is null)
        {

        }
        else if (Encapsulation.IsPathEncapsulation(file.Path.LocalPath))
        {
            var encapsulatorPicker = new SaveToERFWindow();
            encapsulatorPicker.DataContext = new SaveToERFWindowViewModel().LoadModel(file.Path.LocalPath, ResourceTypes);


            var resource = await encapsulatorPicker.ShowDialog<SaveToERFWindowDialogResult>(this);
            if (resource is not null)
            {
                SaveToFile(file.Path.LocalPath, resource.ResRef, resource.ResourceType);
            }
        }
        else
        {
            SaveToFile(file.Path.LocalPath);
        }
    }

    protected abstract void LoadFromFile();
    protected abstract void LoadFromFile(string filepath);
    protected abstract void LoadFromFile(string filepath, ResRef resref, ResourceType resourceType);

    protected abstract void SaveToFile();
    protected abstract void SaveToFile(string filepath);
    protected abstract void SaveToFile(string filepath, ResRef resref, ResourceType resourceType);
}
