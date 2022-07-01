using System.Globalization;

namespace Fmbm.Text;

public static class Csv
{
    static public CultureInfo DefaultCulture { get; } = Cell.DefaultCulture;
    static public StringComparer HeaderComparer { get; } =
        StringComparer.InvariantCultureIgnoreCase;

    public static IEnumerable<TItem> GetItems<TItem>(
        Table table,
        Func<Func<string, Cell>, TItem> itemMaker)
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
            yield return itemMaker(getCell);
        }
    }

    public static IEnumerable<TItem> GetItems<TItem>(
        string csvText,
        Func<Func<string, Cell>, TItem> itemMaker)
    {
        return GetItems(csvText, DefaultCulture, itemMaker);
    }


    public static IEnumerable<TItem> GetItems<TItem>(
        string csvText,
        CultureInfo culture,
        Func<Func<string, Cell>, TItem> itemMaker)
    {
        var table = GetTable(csvText, culture);
        VerifyItemTable(table);
        return GetItems(table, itemMaker);
    }

    internal static Table GetTable<TItem>(
        IEnumerable<TItem> items,
        params (string colName, Func<TItem, object> getColValue)[] colInfos)
    {
        return GetTable(items, DefaultCulture, colInfos);
    }

    internal static Table GetTable<TItem>(
            IEnumerable<TItem> items,
            CultureInfo culture,
            params (string colName, Func<TItem, object> getColValue)[] colInfos)
    {
        var rows = new List<Row>();
        var headers = colInfos.Select(ci => new Cell(ci.colName, culture));
        rows.Add(new Row(headers));
        foreach (var item in items)
        {
            var values = colInfos.Select(ci => ci.getColValue(item));
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
        params (string, Func<TItem, object>)[] colInfos)
    {
        return GetText(items, DefaultCulture, colInfos);
    }

    public static string GetText<TItem>(
        IEnumerable<TItem> items,
        CultureInfo culture,
        params (string, Func<TItem, object>)[] colInfos)
    {
        var table = GetTable(items, culture, colInfos);
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
