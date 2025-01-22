using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Kotor.NET.Common.Data;
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
        init => this.RaiseAndSetIfChanged(ref _resourceTypeOptions, value);
    }

    public SaveToERFWindowViewModel(IEncapsulation encapsulator, IEnumerable<ResourceType> resourceTypeOptions)
    {
        ResourceList = new(encapsulator);
        ResourceTypeOptions = resourceTypeOptions.ToArray();
        ResRef = "";
        ResourceType = _resourceTypeOptions.First();

        this.ObservableForProperty(x => x.ResRef)
            .Subscribe(x => ResourceList.ResRefFilter = x.Value);

        this.ObservableForProperty(x => x.ResourceType)
            .Subscribe(x => ResourceList.ResourceTypeFilter = [x.Value]);
    }
}
