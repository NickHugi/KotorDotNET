using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Kotor.NET.Exceptions;
using Kotor.NET.Resources.Kotor2DA;

namespace Kotor.NET.Formats.Binary2DA.Serialisation;

public class TwoDABinaryDeserializer
{
    private TwoDABinary _binary { get; }

    public TwoDABinaryDeserializer(TwoDABinary binary)
    {
        _binary = binary;
    }

    public TwoDA Deserialize()
    {
        try
        {
            var twoda = new TwoDA();

            var columnCount = _binary.ColumnHeaders.Count;
            var rowCount = _binary.RowHeaders.Count;
            var cellCount = columnCount * rowCount;

            _binary.ColumnHeaders.ForEach(twoda.AddColumn);

            for (int rowIndex = 0; rowIndex < rowCount; rowIndex++)
            {
                var rowHeader = _binary.RowHeaders[rowIndex];
                var row = twoda.AddRow(rowHeader);

                for (int columnIndex = 0; columnIndex < columnCount; columnIndex++)
                {
                    var cellIndex = rowIndex * columnCount + columnIndex;
                    var columnHeader = _binary.ColumnHeaders[columnIndex];
                    var cellValue = _binary.CellValues[cellIndex];
                    row.GetCell(columnHeader).SetString(cellValue);
                }
            }

            return twoda;
        }
        catch (Exception e)
        {
            throw new DeserializationException("Failed to deserialize the 2DA data.", e);
        }
    }
}
