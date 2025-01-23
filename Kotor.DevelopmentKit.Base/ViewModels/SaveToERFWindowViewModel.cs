using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Concurrency;
using System.Text;
using System.Threading.Tasks;
using Avalonia.ReactiveUI;
using Kotor.NET.Common.Data;
using Kotor.NET.Encapsulations;
using Kotor.NET.Tests.Encapsulation;
using ReactiveUI;

namespace Kotor.DevelopmentKit.Base.ViewModels;

public class SaveToERFWindowViewModel : ReactiveObject
{
    private ERFResourceListViewModel _resourceList = default!;
    public ERFResourceListViewModel ResourceList
    {
        get => _resourceList;
        private set => this.RaiseAndSetIfChanged(ref _resourceList, value);
    }

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

    private ResourceType[] _resourceTypeOptions = default!;
    public ResourceType[] ResourceTypeOptions
    {
        get => _resourceTypeOptions;
        private set => this.RaiseAndSetIfChanged(ref _resourceTypeOptions, value);
    }

    public SaveToERFWindowViewModel()
    {
        this.ObservableForProperty(x => x.ResRef)
            .Subscribe(x =>
             {
                 if (ResourceList is not null)
                 {
                     ResourceList.ResRefFilter = x.Value;
                 }
             });

        this.ObservableForProperty(x => x.ResourceType)
            .Subscribe(x =>
            {
                if (ResourceList is not null)
                {
                    ResourceList.ResourceTypeFilter = [x.Value];
                }
            });
    }

    public SaveToERFWindowViewModel LoadModel(string filepath, IEnumerable<ResourceType> resourceTypeOptions)
    {
        ResourceTypeOptions = resourceTypeOptions.ToArray();
        ResourceType = _resourceTypeOptions.First();
        ResRef = "";
        ResourceList = new();

        Task.Run(() =>
        {
            var encapsulator = Encapsulation.LoadFromPath(filepath);
            AvaloniaScheduler.Instance.Schedule(() => ResourceList.LoadModel(encapsulator, null));
        });

        return this;
    }
}
