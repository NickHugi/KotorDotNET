using System;
using System.Collections.Generic;
using DynamicData;

namespace Kotor.DevelopmentKit.Base;

public class SourceListIndexComperer<T> : IComparer<T> where T : notnull
{
    private SourceList<T> _sauce;

    public SourceListIndexComperer(SourceList<T> sauce)
    {
        _sauce = sauce;
    }

    public int Compare(T? x, T? y)
    {
        var a = _sauce.Items.IndexOf(x);
        var b = _sauce.Items.IndexOf(y);
        return a - b;
    }
}
