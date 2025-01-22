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
using Kotor.NET.Encapsulations;
using Kotor.NET.Tests.Encapsulation;
using ReactiveUI;

namespace Kotor.DevelopmentKit.Base.ViewModels;

public class LoadFromERFWindowViewModel : ERFResourceListViewModel
{
    public LoadFromERFWindowViewModel(IEncapsulation encapsulator) : base(encapsulator)
    {
    }
}
