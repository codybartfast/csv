using System.Globalization;
namespace Fmbm.Text;

public class Row
{
    public const string Comma = ",";

    public List<Cell> Cells { get; set; }

    public Row(int length, CultureInfo? ci = null)
    : this(new string[length], ci)
    { }

    public Row(IEnumerable<string> objects, CultureInfo? ci = null)
    : this(objects.Select(o => new Cell(o, ci ?? Cell.DefaultCulture)))
    { }

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

    public string[] ToStringArray()
    {
        return Cells.Select(c => c.Text).ToArray();
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
