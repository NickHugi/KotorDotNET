using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Kotor.NET.Resources.Kotor2DA;
using ReactiveUI;

namespace Kotor.DevelopmentKit.Core;

public abstract class ResourceEditorViewModelBase<T, U> : ReactiveObject where U : new()
{
    private string? _filepath;
    public string? FilePath
    {
        get => _filepath;
        set => this.RaiseAndSetIfChanged(ref _filepath, value);
    }
    public bool FilePathAssigned => FilePath is not null;

    private T _resource;
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

    public void LoadFromFile(string filepath)
    {
        FilePath = filepath;
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
