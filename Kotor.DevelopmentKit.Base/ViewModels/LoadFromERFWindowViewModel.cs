using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reactive.Concurrency;
using System.Reactive.Linq;
using System.Text;
using System.Threading.Tasks;
using Avalonia.ReactiveUI;
using DynamicData;
using Kotor.DevelopmentKit.Base.Windows;
using Kotor.NET.Common.Data;
using Kotor.NET.Encapsulations;
using Kotor.NET.Tests.Encapsulation;
using ReactiveUI;

namespace Kotor.DevelopmentKit.Base.ViewModels;

public class LoadFromERFWindowViewModel : ReactiveObject
{
    private ERFResourceListViewModel _resourceList = default!;
    public ERFResourceListViewModel ResourceList
    {
        get => _resourceList;
        private set => this.RaiseAndSetIfChanged(ref _resourceList, value);
    }

    public LoadFromERFWindowViewModel()
    {
    }

    public LoadFromERFWindowViewModel LoadModel(string filepath, IEnumerable<ResourceType> resourceTypeFilter)
    {
        ResourceList = new();

        Task.Run(() =>
        {
            var encapsulator = Encapsulation.LoadFromPath(filepath);
            AvaloniaScheduler.Instance.Schedule(() => ResourceList.LoadModel(encapsulator, resourceTypeFilter));
        });

        return this;
    }
}
