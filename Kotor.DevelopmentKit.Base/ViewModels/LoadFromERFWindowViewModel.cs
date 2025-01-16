using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DynamicData;
using Kotor.NET.Tests.Encapsulation;
using ReactiveUI;

namespace Kotor.DevelopmentKit.Base.ViewModels;

public class LoadFromERFWindowViewModel : ReactiveObject
{
    private ObservableCollection<ResourceViewModel> _resources = new();
    public ObservableCollection<ResourceViewModel> Resources
    {
        get => _resources;
        set => this.RaiseAndSetIfChanged(ref _resources, value);
    }

    public LoadFromERFWindowViewModel()
    {
        
    }

    public void LoadModel(IEncapsulatedFormat encapsulator)
    {
        _resources.Clear();
        _resources.AddRange(encapsulator.Select(x => new ResourceViewModel
        {
            Filepath = x.FilePath,
            ResRef = x.ResRef,
            Type = x.Type,
            Size = x.Size,
            Offset = x.Offset,
        }));
    }
}
