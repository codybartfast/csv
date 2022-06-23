using System.Collections.Immutable;

namespace Fmbm.Csv;

class Row
{
    ImmutableArray<Cell> Cells { get; }

    public Row(Cell[] cells)
    {
        this.Cells = ImmutableArray.Create(cells);
    }
}