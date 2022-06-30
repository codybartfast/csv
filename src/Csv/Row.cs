namespace Fmbm.Text;

public class Row
{
    public const string Comma = ",";

    public List<Cell> Cells { get; }

    public Row(IEnumerable<Cell> cells)
    {
        this.Cells = new List<Cell>(cells);
    }

    public int Length => Cells.Count;

    public Cell this[int idx]
    {
        get => Cells[idx];
        set => Cells[idx] = value;
    }

    public override string ToString()
    {
        return String.Join(Comma, Cells.Select(c => c.ToString()));
    }

    public string ToCsvText()
    {
        return String.Join(Comma, Cells.Select(c => c.ToCsvText()));
    }
}
