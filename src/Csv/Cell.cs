using System.Globalization;
using System.Text.RegularExpressions;

namespace Fmbm.Text;

public class Cell
{
    internal string Text { get; init; }

    internal Cell(string text)
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
    internal static string Encode(string naiveText)
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

    public static implicit operator string(Cell cell)
    {
        return cell.Text;
    }

    public static implicit operator DateTime(Cell cell)
    {
        // if (DateTime.TryParseExact(
        //     cell.Text,
        //     "yyyy-MM-dd HH:mm",
        //     null,
        //     DateTimeStyles.None,
        //     out var dt))
        // {
        //     return dt;
        // }
        return DateTime.Parse(cell.Text);
    }

    public static implicit operator int(Cell cell)
    {
        return int.Parse(cell.Text, NumberStyles.AllowThousands);
    }

    public override string ToString()
    {
        return Text;
    }
}