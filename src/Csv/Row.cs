using System.Collections.Immutable;

namespace Fmbm.Text;

class Row
{
    internal ImmutableArray<Cell> Cells { get; }

    internal Row(IEnumerable<Cell> cells)
    {
        this.Cells = ImmutableArray.Create(cells.ToArray());
    }

    public Cell Lookup(Func<string, int> getIndex, string header)
    {
        return Cells[getIndex(header)];
    }

    public override string ToString()
    {
        return String.Join(",", Cells);
    }
}