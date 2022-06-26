using System.Collections.Immutable;

namespace Fmbm.Text;

class Row
{
    internal ImmutableArray<Cell> Cells { get; }

    internal Row(IEnumerable<Cell> cells)
    {
        this.Cells = ImmutableArray.Create(cells.ToArray());
    }

    public int Length => Cells.Length;

    public Cell this[int idx] => Cells[idx];

    public override string ToString()
    {
        return String.Join(",", Cells);
    }
}