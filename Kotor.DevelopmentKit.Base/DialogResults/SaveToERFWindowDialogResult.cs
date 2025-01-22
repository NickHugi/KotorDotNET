using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Kotor.NET.Common.Data;

namespace Kotor.DevelopmentKit.Base.DialogResults;

public class SaveToERFWindowDialogResult
{
    public required string ResRef { get; init; }
    public required ResourceType ResourceType { get; init; }
}
