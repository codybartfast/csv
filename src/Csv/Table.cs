namespace Fmbm.Text;

public class Table
{
    public const string NewLine = "\n";

    public List<Row> Rows { get; }

    public Table(IEnumerable<Row> rows)
    {
        this.Rows = new List<Row>(rows);
    }

    public int Length => Rows.Count;

    public Row this[int idx]
    {
        get => Rows[idx];
        set => Rows[idx] = value;
    }

    public override string ToString()
    {
        return String.Join(NewLine, Rows.Select(r => r.ToString()));
    }

    public string ToCsvText()
    {
        return String.Join(NewLine, Rows.Select(r => r.ToCsvText()));
    }
}
