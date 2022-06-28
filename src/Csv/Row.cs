using System.Collections.Immutable;

namespace Fmbm.Text;

public class Row
{
    public ImmutableArray<Cell> Cells { get; }

    public Row(IEnumerable<Cell> cells)
    {
        this.Cells = ImmutableArray.Create(cells.ToArray());
    }

    public int Length => Cells.Length;

    public Cell this[int idx] => Cells[idx];

    public override string ToString()
    {
        return String.Join(",", Cells);
    }

    public string ToCsvText()
    {
        return String.Join(",", Cells.Select(c => c.ToCsvText()));
    }
}