using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reactive;
using System.Reactive.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Avalonia.Controls;
using Avalonia.ReactiveUI;
using DynamicData;
using DynamicData.Binding;
using Kotor.NET.Common.Data;
using Kotor.NET.Encapsulations;
using Kotor.NET.Extensions;
using Kotor.NET.Tests.Encapsulation;
using ReactiveUI;

namespace Kotor.DevelopmentKit.Base.ViewModels;

public class ERFResourceListViewModel : ReactiveObject
{
    public IEncapsulation Encapsulator { get; private set; }

    private SourceList<EncapsulatedResourceViewModel> _resourcesSource = new();
    private readonly ReadOnlyObservableCollection<EncapsulatedResourceViewModel> _resources;
    public ReadOnlyObservableCollection<EncapsulatedResourceViewModel> Resources => _resources;

    private EncapsulatedResourceViewModel? _selectedItem = null;
    public EncapsulatedResourceViewModel? SelectedItem
    {
        get => _selectedItem;
        set => this.RaiseAndSetIfChanged(ref _selectedItem, value);
    }

    public string _resrefFilter = "";
    public string ResRefFilter
    {
        get => _resrefFilter;
        set => this.RaiseAndSetIfChanged(ref _resrefFilter, value);
    }

    public ResourceType[]? _typeFilter = null;
    public ResourceType[]? ResourceTypeFilter
    {
        get => _typeFilter;
        set => this.RaiseAndSetIfChanged(ref _typeFilter, value);
    }

    private bool _loading = true;
    public bool Loading
    {
        get => _loading;
        set => this.RaiseAndSetIfChanged(ref _loading, value);
    }

    public ERFResourceListViewModel()
    {
        Encapsulator = default!;
        _resources = default!;

        var filter = this.WhenValueChanged(x => x.ResRefFilter)
            .Throttle(TimeSpan.FromMilliseconds(50), AvaloniaScheduler.Instance)
            .DistinctUntilChanged()
            .Select(CreatePredicate);

        _resourcesSource.Connect()
            .RefCount()
            .Filter(filter)
            // todo - readd type filter
            // todo - readd sorting
            .Bind(out _resources)
            .DisposeMany()
            .Subscribe();
    }

    public ERFResourceListViewModel LoadModel(IEncapsulation encapsulator, IEnumerable<ResourceType>? resourceTypeFilter)
    {
        Encapsulator = encapsulator;

        _typeFilter = resourceTypeFilter?.ToArray();

        _resourcesSource.Clear();
        _resourcesSource.AddRange(Encapsulator.Select(x => new EncapsulatedResourceViewModel
        {
            Filepath = x.FilePath,
            ResRef = x.ResRef,
            Type = x.Type,
            Size = x.Size,
            Offset = x.Offset,
        }));

        Loading = false;

        return this;
    }

    public Func<EncapsulatedResourceViewModel, bool> CreatePredicate(string text)
    {
        return x => string.IsNullOrEmpty(ResRefFilter) || x.ResRef.Contains(ResRefFilter);
    }
}
