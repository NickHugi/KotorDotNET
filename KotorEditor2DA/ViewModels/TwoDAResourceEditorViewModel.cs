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
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Kotor.DevelopmentKit.Editor2DA.ViewModels;

public class TwoDAResourceEditorViewModel : ResourceEditorViewModelBase<TwoDAViewModel, TwoDA>
{
    public TwoDAResourceEditorViewModel()
    {
        Resource = new();
    }


    public override byte[] SerializeModelToBytes()
    {
        var twoda = BuildModel();
        using var memoryStream = new MemoryStream();
        new TwoDABinarySerializer(twoda).Serialize().Write(memoryStream);
        return memoryStream.ToArray();
    }
    public override void SerializeModelToFile()
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

    public override TwoDA DeserializeModel(byte[] bytes)
    {
        return TwoDA.FromBytes(bytes);
    }
    public override TwoDA DeserializeModel(string path)
    {
        return TwoDA.FromFile(path);
    }
}
