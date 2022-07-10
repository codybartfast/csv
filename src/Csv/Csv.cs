using System.Globalization;

namespace Fmbm.Text;

public static class Csv
{
    static public CultureInfo DefaultCulture { get; } = Cell.DefaultCulture;
    static public StringComparer HeaderComparer { get; } =
        StringComparer.InvariantCultureIgnoreCase;

    public static IEnumerable<TItem> GetItems<TItem>(
        Table table,
        Func<Func<string, Cell>, TItem> makeItem)
    {
        var headers = table.Rows[0].Cells.Select(c => c.Text).ToArray();
        var dict = new Dictionary<string, int>(HeaderComparer);
        for (int i = 0; i < headers.Length; i++)
        {
            dict.Add(headers[i].Trim(), i);
        }
        Func<string, int> getIndex = header => dict[header];
        foreach (var row in table.Rows.Skip(1))
        {
            Func<string, Cell> getCell = header => row[getIndex(header)];
            yield return makeItem(getCell);
        }
    }

    public static IEnumerable<TItem> GetItems<TItem>(
        string csvText,
        Func<Func<string, Cell>, TItem> makeItem)
    {
        return GetItems(csvText, DefaultCulture, makeItem);
    }


    public static IEnumerable<TItem> GetItems<TItem>(
        string csvText,
        CultureInfo culture,
        Func<Func<string, Cell>, TItem> makeItem)
    {
        var table = GetTable(csvText, culture);
        VerifyItemTable(table);
        return GetItems(table, makeItem);
    }

    internal static Table GetTable<TItem>(
        IEnumerable<TItem> items,
        params (string header, Func<TItem, object> getValue)[] columnInfos)
    {
        return GetTable(items, DefaultCulture, columnInfos);
    }

    internal static Table GetTable<TItem>(
            IEnumerable<TItem> items,
            CultureInfo culture,
            params (string header, Func<TItem, object> getValue)[] columnInfos)
    {
        var rows = new List<Row>();
        var headers = columnInfos.Select(ci => new Cell(ci.header, culture));
        rows.Add(new Row(headers));
        foreach (var item in items)
        {
            var values = columnInfos.Select(ci => ci.getValue(item));
            rows.Add(new Row(values.Select(str => new Cell(str, culture))));
        }
        return new Table(rows);
    }

    public static Table GetTable(string csvText)
    {
        return GetTable(csvText, DefaultCulture);
    }

    public static Table GetTable(string csvText, CultureInfo culture)
    {
        return TextParser.GetTable(csvText, culture);
    }

    public static string GetText(Table table)
    {
        return table.ToCsvText();
    }

    public static string GetText<TItem>(
        IEnumerable<TItem> items,
        params (string, Func<TItem, object>)[] columnInfos)
    {
        return GetText(items, DefaultCulture, columnInfos);
    }

    public static string GetText<TItem>(
        IEnumerable<TItem> items,
        CultureInfo culture,
        params (string, Func<TItem, object>)[] columnInfos)
    {
        var table = GetTable(items, culture, columnInfos);
        return GetText(table);
    }

    public static void VerifyItemTable(Table table)
    {
        if (table.Length == 0)
        {
            throw new CsvException("Table has no rows");
        }

        var headers = table.Rows[0].Cells.Select(c => c.Text).ToArray();
        var grouped = headers.GroupBy(h => h, HeaderComparer).ToArray();
        var dup = grouped.FirstOrDefault(g => g.Count() > 1);
        if (dup is not null)
        {
            throw new CsvException($"Duplicate header: {dup.Key}");
        }

        var width = headers.Length;
        for (int i = 0; i < table.Length; i++)
        {
            var rowWidth = table[i].Length;
            if (rowWidth != width)
            {
                throw new CsvException(
                    $"Row {i} has {rowWidth} values but there are {width} headers.");
            }
        }
    }
}
