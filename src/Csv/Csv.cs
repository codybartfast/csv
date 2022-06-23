
using System.Text;

namespace Fmbm.Csv;

public static class Csv
{

    public static string GetText<TItem>(
        IEnumerable<TItem> items,
        params (string, Func<TItem, object>)[] colInfos
        )
    {
        var rows = new List<string>();
        var headers = colInfos.Select(ci => ci.Item1).ToArray();
        rows.Add(String.Join(",", headers));
        foreach (var item in items)
        {
            var values = colInfos.Select(ci => ci.Item2(item).ToString());
            rows.Add(String.Join(",", values));
        }
        return string.Join("\r\n", rows);
    }
}