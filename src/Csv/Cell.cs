using System.Globalization;
using System.Text.RegularExpressions;

namespace Fmbm.Text;


public class Cell
{
    const NumberStyles IntStyles =
        NumberStyles.Integer |
        NumberStyles.AllowLeadingWhite |
        NumberStyles.AllowTrailingWhite |
        NumberStyles.AllowThousands;
    const NumberStyles FloatStyles =
        NumberStyles.Float |
        NumberStyles.AllowLeadingWhite |
        NumberStyles.AllowTrailingWhite |
        NumberStyles.AllowThousands;

    static readonly CultureInfo culture = CultureInfo.InvariantCulture;

    internal string Text { get; init; }

    internal Cell(string text)
    {
        this.Text = text;
    }

    internal static Cell From(object value)
    {
        string text;
        switch (value)
        {
            case string str:
                text = str;
                break;
            case DateTime dt:
                text = dt.ToString("yyyy-MM-dd HH:mm");
                break;
            case int n:
                text = n.ToString();
                break;
            case uint n:
                text = n.ToString();
                break;
            case long n:
                text = n.ToString();
                break;
            case ulong n:
                text = n.ToString();
                break;
            case float n:
                text = n.ToString();
                break;
            case double n:
                text = n.ToString();
                break;
            case decimal n:
                text = n.ToString();
                break;
            default:
                throw new CsvException(
                    $"Cannot covert {value.GetType().Name} to a CSV value.");
        };
        return new Cell(Escape(text));
    }

    static readonly Regex needsEscaping =
        new Regex("[,\"\n]", RegexOptions.Compiled);
    internal static string Escape(string naiveText)
    {
        var dq = "\"";
        var dqdq = "\"\"";
        if (needsEscaping.IsMatch(naiveText))
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
        return DateTime.Parse(cell.Text, culture);
    }

    public static implicit operator int(Cell cell)
    {
        return int.Parse(cell.Text, IntStyles, culture);
    }

    public static implicit operator uint(Cell cell)
    {
        return uint.Parse(cell.Text, IntStyles, culture);
    }

    public static implicit operator long(Cell cell)
    {
        return long.Parse(cell.Text, IntStyles, culture);
    }

    public static implicit operator ulong(Cell cell)
    {
        return ulong.Parse(cell.Text, IntStyles, culture);
    }

    public static implicit operator float(Cell cell)
    {
        return float.Parse(cell.Text, FloatStyles, culture);
    }

    public static implicit operator double(Cell cell)
    {
        return double.Parse(cell.Text, FloatStyles, culture);
    }

    public static implicit operator decimal(Cell cell)
    {
        return decimal.Parse(cell.Text, FloatStyles, culture);
    }

    public override string ToString()
    {
        return Text;
    }
}