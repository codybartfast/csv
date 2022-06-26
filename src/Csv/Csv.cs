using System.Text.RegularExpressions;

namespace Fmbm.Text;

public static class Csv
{
    public static IEnumerable<TItem> GetItems<TItem>(
        string csv,
        Func<Func<string, Cell>, TItem> maker)
    {
        var table = TableReader.GetTable(csv);
        var headers = table.Rows[0].Cells.Select(c => c.Text).ToArray();
        var dict = new Dictionary<string, int>(
            StringComparer.InvariantCultureIgnoreCase);
        for (int i = 0; i < headers.Length; i++)
        {
            dict.Add(headers[i].Trim(), i);
        }
        Func<string, int> getIndex = header => dict[header];
        foreach (var row in table.Rows.Skip(1))
        {
            Func<string, Cell> getCell = header => row[getIndex(header)];
            yield return maker(getCell);
        }
    }

    public static string GetText<TItem>(
        IEnumerable<TItem> items,
        params (string, Func<TItem, object>)[] colInfos)
    {
        return GetTable(items, colInfos).ToString();
    }

    static Table GetTable<TItem>(
        IEnumerable<TItem> items,
        params (string, Func<TItem, object>)[] colInfos)
    {
        var rows = new List<Row>();
        var headers = colInfos.Select(ci => Cell.From(ci.Item1));
        rows.Add(new Row(headers));
        foreach (var item in items)
        {
            var values = colInfos.Select(ci => ci.Item2(item));
            rows.Add(new Row(values.Select(str => Cell.From(str))));
        }
        return new Table(rows);
    }

}
