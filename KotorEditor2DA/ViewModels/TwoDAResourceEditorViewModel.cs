using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DynamicData;
using Kotor.DevelopmentKit.Base.ViewModels;
using Kotor.NET.Encapsulations;
using Kotor.NET.Formats.Binary2DA.Serialisation;
using Kotor.NET.Resources.Kotor2DA;
using Kotor.NET.Tests.Encapsulation;
using ReactiveUI;

namespace Kotor.DevelopmentKit.Editor2DA.ViewModels;

public class TwoDAResourceEditorViewModel : ResourceEditorViewModelBase<TwoDAViewModel, TwoDA>
{
    public TwoDAResourceEditorViewModel()
    {
        Resource = new();
    }

    public override void LoadFromFile()
    {
        if (FilePath.ToLower().EndsWith(".rim"))
        {
            var capsule = Encapsulation.LoadFromPath(FilePath);
            var data = capsule.Read(ResRef, ResourceType);
            var twoda = TwoDA.FromBytes(data);
            LoadModel(twoda);
        }
        else
        {
            var twoda = TwoDA.FromFile(FilePath);
            LoadModel(twoda);
        }
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
