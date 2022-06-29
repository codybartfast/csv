using System.Collections.Immutable;

namespace Fmbm.Text;

public class Table
{
    public Row[] Rows { get; }

    public Table(IEnumerable<Row> rows)
    {
        this.Rows = rows.ToArray();
    }

    public int Length => Rows.Length;

    public Row this[int idx]
    {
        get => Rows[idx];
        set => Rows[idx] = value;
    }

    public override string ToString()
    {
        return String.Join("\r\n", Rows.Select(r => r.ToString()));
    }

    public string ToCsvText()
    {
        return String.Join("\r\n", Rows.Select(r => r.ToCsvText()));
    }
}