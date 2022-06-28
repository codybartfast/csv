using System.Collections.Immutable;

namespace Fmbm.Text;

public class Table
{
    public ImmutableArray<Row> Rows { get; }

    public Table(IEnumerable<Row> rows)
    {
        this.Rows = ImmutableArray.Create(rows.ToArray());
    }

    public int Length => Rows.Length;

    public Row this[int idx] => Rows[idx];

    public override string ToString()
    {
        return String.Join("\r\n", Rows);
    }

    public string ToCsvText()
    {
        return String.Join("\r\n", Rows.Select(r => r.ToCsvText()));
    }
}