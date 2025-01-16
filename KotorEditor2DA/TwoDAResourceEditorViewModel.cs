using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DynamicData;
using Kotor.DevelopmentKit.Base.ViewModels;
using Kotor.NET.Formats.Binary2DA.Serialisation;
using Kotor.NET.Resources.Kotor2DA;
using ReactiveUI;

namespace Kotor.DevelopmentKit.Editor2DA;

public class TwoDAResourceEditorViewModel : ResourceEditorViewModelBase<TwoDAViewModel, TwoDA>
{
    public TwoDAResourceEditorViewModel()
    {
        Resource = new();
    }

    public override void LoadFromFile()
    {
        var twoda = TwoDA.FromFile(FilePath);
        LoadModel(twoda);
    }

    public override void SaveToFile()
    {
        var twoda = BuildModel();
        using var fileStream = File.OpenWrite(FilePath);
        new TwoDABinarySerializer(twoda).Serialize().Write(fileStream);
    }

    public override TwoDA BuildModel()
    {
        return Resource.Build();
    }
    public override void LoadModel(TwoDA model)
    {
        Resource.Filter = "";
        Resource.Load(model);
    }
}
