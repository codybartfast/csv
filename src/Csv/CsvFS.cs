using System.Globalization;

using Microsoft.FSharp.Core;

namespace Fmbm.Text;


public static partial class Csv
{

    static Func<Func<string, Cell>, TItem> CSMakeItem<TItem>(
        FSharpFunc<FSharpFunc<string, Cell>, TItem> makeItem)
    {
        TItem MakeItem(Func<string, Cell> row)
        {
            var converter = new Converter<string, Cell>(row);
            var fsRow = FSharpFunc<string, Cell>.FromConverter(converter);
            return makeItem.Invoke(fsRow);
        }
        return MakeItem;
    }

    static (string, Func<TItem, object>)[] CSColumnInfos<TItem>(
        IEnumerable<(string, FSharpFunc<TItem, object>)> fsColumnInfos)
    {
        return
            fsColumnInfos
            .Select<(string, FSharpFunc<TItem, object>), (string, Func<TItem, object>)>(
                colInfo => (colInfo.Item1, colInfo.Item2.Invoke))
            .ToArray();
    }


    public static IEnumerable<TItem> getItems<TItem>(
        string csvText,
        FSharpFunc<FSharpFunc<string, Cell>, TItem> makeItem)
    {
        return GetItems(csvText, DefaultCulture, CSMakeItem(makeItem));
    }

    public static IEnumerable<TItem> getItems<TItem>(
        string csvText,
        CultureInfo culture,
        FSharpFunc<FSharpFunc<string, Cell>, TItem> makeItem)
    {
        return GetItems(csvText, culture, CSMakeItem(makeItem));
    }

    public static IEnumerable<TItem> getItems<TItem>(
        Table table,
        FSharpFunc<FSharpFunc<string, Cell>, TItem> makeItem)
    {
        return GetItems(table, CSMakeItem(makeItem));
    }


    public static string getText<TItem>(
        IEnumerable<TItem> items,
        IEnumerable<(string, FSharpFunc<TItem, object>)> columnInfos)
    {
        return GetText(items, DefaultCulture, CSColumnInfos(columnInfos));
    }

    public static string getText<TItem>(
        IEnumerable<TItem> items,
        CultureInfo culture,
        IEnumerable<(string, FSharpFunc<TItem, object>)> columnInfos)
    {
        return GetText(items, culture, CSColumnInfos(columnInfos));
    }

    public static string getText(Table table)
    {
        return GetText(table);
    }


    public static Table getTable(string csvText)
    {
        return GetTable(csvText);
    }

    public static Table getTable(string csvText, CultureInfo culture)
    {
        return GetTable(csvText, culture);
    }

    public static Table getTable<TItem>(
            IEnumerable<TItem> items,
            IEnumerable<(string header, FSharpFunc<TItem, object> getValue)> columnInfos)
    {
        return GetTable(items, CSColumnInfos(columnInfos));
    }

    internal static Table getTable<TItem>(
        IEnumerable<TItem> items,
        CultureInfo culture,
        IEnumerable<(string header, FSharpFunc<TItem, object> getValue)> columnInfos)
    {
        return GetTable(items, culture, CSColumnInfos(columnInfos));
    }

}