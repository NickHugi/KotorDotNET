using System;
using System.Collections.Generic;
using DynamicData;

namespace Kotor.DevelopmentKit.Base;

public class SourceListIndexComperer<T> : IComparer<T> where T : notnull
{
    private SourceList<T> _source;

    public SourceListIndexComperer(SourceList<T> sauce)
    {
        _source = sauce;
    }

    public int Compare(T? x, T? y)
    {
        var a = _source.Items.IndexOf(x);
        var b = _source.Items.IndexOf(y);
        return a - b;
    }
}
