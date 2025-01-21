using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Kotor.NET.Common.Data;
using Kotor.NET.Resources.Kotor2DA;
using ReactiveUI;

namespace Kotor.DevelopmentKit.Base.ViewModels;

public abstract class ResourceEditorViewModelBase<T, U> : ReactiveObject where U : new()
{
    private string? _filepath = default!;
    public string? FilePath
    {
        get => _filepath;
        set => this.RaiseAndSetIfChanged(ref _filepath, value);
    }
    public bool FilePathAssigned => FilePath is not null;

    private string _resref = default!;
    public string ResRef
    {
        get => _resref;
        set => this.RaiseAndSetIfChanged(ref _resref, value);
    }

    private ResourceType _resourceType = default!;
    public ResourceType ResourceType
    {
        get => _resourceType;
        set => this.RaiseAndSetIfChanged(ref _resourceType, value);
    }

    private T _resource = default!;
    public T Resource
    {
        get => _resource;
        set => this.RaiseAndSetIfChanged(ref _resource, value);
    }

    public void NewFile()
    {
        FilePath = null;
        LoadModel(new());
    }

    public void LoadFromFile(string filepath, ResRef resref, ResourceType resourceType)
    {
        FilePath = filepath;
        ResRef = resref.Get();
        ResourceType = resourceType;
        LoadFromFile();
    }
    public void LoadFromFile(string filepath)
    {
        FilePath = filepath;
        ResRef = Path.GetFileNameWithoutExtension(filepath);
        ResourceType = ResourceType.ByExtension(Path.GetExtension(filepath).Replace(".", ""));
        LoadFromFile();
    }
    public abstract void LoadFromFile();

    public void SaveToFile(string filepath)
    {
        FilePath = filepath;
        SaveToFile();
    }
    public abstract void SaveToFile();

    public abstract void LoadModel(U model);
    public abstract U BuildModel();
}
