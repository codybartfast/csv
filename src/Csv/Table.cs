using System.Collections.Immutable;

namespace Fmbm.Csv;

class Table
{
    ImmutableArray<Row> Rows { get; }

    public Table(Row[] rows)
    {
        this.Rows = ImmutableArray.Create(rows);
    }
}