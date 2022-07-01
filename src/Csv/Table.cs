using System.Text;

namespace Fmbm.Text;

public class Table
{
    static string JoinPlusSeparator(string sep, IEnumerable<string> strings)
    {
        var sb = new StringBuilder();
        foreach (var str in strings)
        {
            sb.Append(str);
            sb.Append(sep);
        }
        return sb.ToString();
    }

    public const string NewLine = "\n";

    public List<Row> Rows { get; set; }

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
        return JoinPlusSeparator(NewLine, Rows.Select(r => r.ToString()));
    }

    public string ToCsvText()
    {
        return JoinPlusSeparator(NewLine, Rows.Select(r => r.ToCsvText()));
    }
}
