using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Kotor.NET.Tests.Encapsulation;

namespace Kotor.NET.Encapsulations;

public static class Encapsulation
{
    public static IEncapsulation LoadFromPath(string path)
    {
        var extension = Path.GetExtension(path.ToLower());

        return extension switch
        {
            ".erf" => new ERFEncapsulation(path),
            ".mod" => new ERFEncapsulation(path),
            ".rim" => new RIMEncapsulation(path),
            _ => throw new ArgumentException() // TODO
        };
    }
}
