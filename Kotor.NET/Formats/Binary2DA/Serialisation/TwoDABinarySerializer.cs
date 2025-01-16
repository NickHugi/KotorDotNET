using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Kotor.NET.Exceptions;
using Kotor.NET.Resources.Kotor2DA;

namespace Kotor.NET.Formats.Binary2DA.Serialisation;

public class TwoDABinarySerializer
{
    private TwoDA _twoda { get; }

    public TwoDABinarySerializer(TwoDA twoda)
    {
        _twoda = twoda;
    }

    public TwoDABinary Serialize()
    {
        try
        {
            var binary = new TwoDABinary();

            binary.ColumnHeaders = _twoda.GetColumns().ToList();
            binary.RowHeaders = _twoda.GetRows().Select(x => x.RowHeader).ToList();
            binary.CellValues = _twoda.GetRows().SelectMany(x =>
            {
                return binary.ColumnHeaders.Select(y => x.GetCell(y).AsString());
            }).ToList();

            return binary;
        }
        catch (Exception e) 
        {
            throw new SerializationException("Failed to serialize the 2DA data.", e);
        }
    }
}
