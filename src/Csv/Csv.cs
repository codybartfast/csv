namespace Fmbm.Text;

public static class Csv
{
    public static IEnumerable<TItem> GetItems<TItem>(
        string csvText,
        Func<Func<string, Cell>, TItem> maker)
    {
        var table = GetTable(csvText);
        VerifyTable(table);
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

    internal static Table GetTable(string csvText)
    {
        return CsvParser.GetTable(csvText);
    }

    public static string GetText<TItem>(
        IEnumerable<TItem> items,
        params (string, Func<TItem, object>)[] colInfos)
    {
        return GetTable(items, colInfos).ToCsvText();
    }

    internal static Table GetTable<TItem>(
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

    internal static void VerifyTable(Table table)
    {
        if (table.Length == 0)
        {
            throw new CsvException("Table has no rows");
        }

        var headers = table.Rows[0].Cells.Select(c => c.Text).ToArray();
        var grouped = headers.GroupBy(h => h,
            StringComparer.InvariantCultureIgnoreCase).ToArray();
        var dup = grouped.FirstOrDefault(g => g.Count() > 1);
        if (dup is not null)
        {
            throw new CsvException($"Duplicate header: {dup.Key}");
        }

        var width = headers.Length;
        for (int i = 0; i < table.Length; i++)
        {
            var rowWidth = table[i].Cells.Length;
            if (rowWidth != width)
            {
                throw new CsvException(
                    $"Row {i} has {rowWidth} values but there are {width} headers.");
            }
        }
    }
}
