using System.Collections.Immutable;

namespace Fmbm.Text;

class Table
{
    internal ImmutableArray<Row> Rows { get; }

    internal Table(IEnumerable<Row> rows)
    {
        this.Rows = ImmutableArray.Create(rows.ToArray());
    }

    public override string ToString()
    {
        return String.Join("\r\n", Rows);
    }
}