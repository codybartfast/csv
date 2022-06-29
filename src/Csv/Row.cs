using System.Collections.Immutable;

namespace Fmbm.Text;

public class Row
{
    public Cell[] Cells { get; }

    public Row(IEnumerable<Cell> cells)
    {
        this.Cells = cells.ToArray();
    }

    public int Length => Cells.Length;

    public Cell this[int idx]
    {
        get => Cells[idx];
        set => Cells[idx] = value;
    }

    public override string ToString()
    {
        return String.Join(",", Cells.Select(c => c.ToString()));
    }

    public string ToCsvText()
    {
        return String.Join(",", Cells.Select(c => c.ToCsvText()));
    }
}