using System.Text.RegularExpressions;

namespace Fmbm.Csv;

public class Cell
{
    internal string Text { get; init; }

    Cell(string text)
    {
        this.Text = text;
    }

    internal static Cell From(object value)
    {
        var naiveText = value switch
        {
            null => "",
            string s => s,
            DateTime dt => dt.ToString("yyyy-MM-dd HH:mm"),

            sbyte n => n.ToString(),
            byte n => n.ToString(),
            short n => n.ToString(),
            int n => n.ToString(),
            long n => n.ToString(),
            ushort n => n.ToString(),
            uint n => n.ToString(),
            ulong n => n.ToString(),
            float n => n.ToString(),
            double n => n.ToString(),
            decimal n => n.ToString(),

            _ => throw new CsvException(
                $"Cannot covert {value.GetType().Name} to a CSV value.")
        };

        return new Cell(Encode(naiveText));
    }

    static readonly Regex needsEncoding =
        new Regex("[,\"\r\n]", RegexOptions.Compiled);
    static string Encode(string naiveText)
    {
        var dq = "\"";
        var dqdq = "\"\"";
        if (needsEncoding.IsMatch(naiveText))
        {
            return $"{dq}{naiveText.Replace(dq, dqdq)}{dq}";
        }
        else
        {
            return naiveText;
        }
    }

    public override string ToString()
    {
        return Text;
    }
}