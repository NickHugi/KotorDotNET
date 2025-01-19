using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reactive.Linq;
using System.Text;
using System.Threading.Tasks;
using Avalonia.ReactiveUI;
using DynamicData;
using Kotor.NET.Common.Data;
using Kotor.NET.Tests.Encapsulation;
using ReactiveUI;

namespace Kotor.DevelopmentKit.Base.ViewModels;

public class LoadFromERFWindowViewModel : ReactiveObject
{
    private IEncapsulatedFormat Encapsulator { get; private set; }

    private SourceList<ResourceViewModel> _resourcesSource = new();
    private readonly ReadOnlyObservableCollection<ResourceViewModel> _resources;
    public ReadOnlyObservableCollection<ResourceViewModel> Resources => _resources;

    private ResourceViewModel _selectedItem;
    public ResourceViewModel SelectedItem
    {
        get => _selectedItem;
        set => this.RaiseAndSetIfChanged(ref _selectedItem, value);
    }

    public string _resrefFilter;
    public string ResRefFilter
    {
        get => _resrefFilter;
        set => this.RaiseAndSetIfChanged(ref _resrefFilter, value);
    }

    public ResourceType[] _typeFilter;
    public ResourceType[] TypeFilter
    {
        get => _typeFilter;
        set => this.RaiseAndSetIfChanged(ref _typeFilter, value);
    }

    public LoadFromERFWindowViewModel()
    {
        _resrefFilter = "";
        _resourcesSource = new();
        _resourcesSource.Connect()
            .ObserveOn(AvaloniaScheduler.Instance)
            .AutoRefreshOnObservable(x => this.ObservableForProperty(x => x.ResRefFilter))
            .Filter(resource => resource.ResRef.ToLower().Contains(_resrefFilter.ToLower()))
            .Filter(resource => (_typeFilter is not null) ? _typeFilter.Contains(resource.Type) : true)
            .Bind(out _resources)
            .Subscribe();
    }

    public void LoadModel(IEncapsulatedFormat encapsulator)
    {
        Encapsulator = encapsulator;

        _resourcesSource.Clear();
        _resourcesSource.AddRange(encapsulator.Select(x => new ResourceViewModel
        {
            Filepath = x.FilePath,
            ResRef = x.ResRef,
            Type = x.Type,
            Size = x.Size,
            Offset = x.Offset,
        }));
    }
}
