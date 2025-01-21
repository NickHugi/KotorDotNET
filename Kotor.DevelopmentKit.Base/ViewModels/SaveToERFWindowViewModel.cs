using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Kotor.NET.Tests.Encapsulation;

namespace Kotor.DevelopmentKit.Base.ViewModels;

public class SaveToERFWindowViewModel : ERFResourceListViewModel
{
    public SaveToERFWindowViewModel(IEncapsulation encapsulator) : base(encapsulator)
    {
    }
}
