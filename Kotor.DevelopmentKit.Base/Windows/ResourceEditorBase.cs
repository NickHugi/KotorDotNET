using System;
using System.Collections.Generic;
using System.IO;
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

    protected abstract void LoadFromFile();
    protected abstract void LoadFromFile(string filepath);
    protected abstract void LoadFromFile(string filepath, ResRef resref, ResourceType resourceType);

    protected abstract void SaveToFile();
    protected abstract void SaveToFile(string filepath);
    protected abstract void SaveToFile(string filepath, ResRef resref, ResourceType resourceType);



    protected async Task<SaveToERFWindowDialogResult?> SaveResourcePicker()
    {
        var file = await TopLevel.GetTopLevel(this)!.StorageProvider.SaveFilePickerAsync(FilePickerSaveOptions);

        if (file is null)
        {
            // No file was chosen.
        }
        else if (Encapsulation.IsPathEncapsulation(file.Path.LocalPath))
        {
            var encapsulatorPicker = new SaveToERFWindow();
            encapsulatorPicker.DataContext = new SaveToERFWindowViewModel().LoadModel(file.Path.LocalPath, ResourceTypes);

            return await encapsulatorPicker.ShowDialog<SaveToERFWindowDialogResult>(this);
        }
        else
        {
            var filepath = file.Path.LocalPath;
            var resourceType = ResourceType.ByExtension(Path.GetExtension(filepath).Replace(".", ""));
            var resref = Path.GetFileNameWithoutExtension(filepath);

            return new()
            {
                FilePath = filepath,
                ResourceType = resourceType,
                ResRef = resref,
            };
        }

        // No resource was chosen.
        return null;
    }

    protected async Task<LoadFromERFWindowDialogResult> OpenResourcePicker()
    {
        var files = await TopLevel.GetTopLevel(this)!.StorageProvider.OpenFilePickerAsync(FilePickerOpenOptions);

        var file = files.FirstOrDefault();

        if (file is null)
        {
            // No file was chosen.
        }
        else if (Encapsulation.IsPathEncapsulation(file.Path.LocalPath))
        {
            var encapsulatorPicker = new LoadFromERFWindow();
            encapsulatorPicker.DataContext = new LoadFromERFWindowViewModel().LoadModel(file.Path.LocalPath, ResourceTypes);

            return await encapsulatorPicker.ShowDialog<LoadFromERFWindowDialogResult>(this);
        }
        else
        {
            var filepath = file.Path.LocalPath;
            var resourceType = ResourceType.ByExtension(Path.GetExtension(filepath).Replace(".", ""));
            var resref = Path.GetFileNameWithoutExtension(filepath);

            return new()
            {
                FilePath = filepath,
                ResourceType = resourceType,
                ResRef = resref,
            };
        }

        // No resource was chosen.
        return null;
    }
}
