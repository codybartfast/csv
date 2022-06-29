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

    public string Text { get; init; }

    public Cell(string text)
    {
        this.Text = text;
    }

    public Cell(DateTime dt)
    {
        this.Text = dt.ToString("yyyy-MM-dd HH:mm");
    }

    public Cell(object o)
    {
        this.Text = (o?.ToString()) ?? String.Empty;
    }

    // public static Cell From(object value)
    // {
    //     return new Cell(value);
    // }

    // internal static Cell FromAny(object value)
    // {
    //     return new Cell(value);
    // }

    static bool TryGetText(object value, out string? text)
    {
        switch (value)
        {
            case null:
                text = String.Empty;
                return true;
            case string str:
                text = str;
                return true;
            case DateTime dt:
                text = dt.ToString("yyyy-MM-dd HH:mm");
                return true;
            case int:
            case uint:
            case long:
            case ulong:
            case float:
            case double:
            case decimal:
                text = value.ToString();
                return true;
            default:
                text = null;
                return false;
        };
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

    static readonly Regex needsQuoting =
        new Regex("[,\"\n]", RegexOptions.Compiled);
    public static bool NeedsQuoting(string text)
    {
        return needsQuoting.IsMatch(text);
    }

    public static string Quote(string text)
    {
        var dq = "\"";
        var dqdq = "\"\"";
        return $"{dq}{text.Replace(dq, dqdq)}{dq}";
    }

    public static string QuoteIfNeeded(string text)
    {
        return NeedsQuoting(text) ? Quote(text) : text;
    }

    public override string ToString()
    {
        return Text;
    }

    public string ToCsvText()
    {
        return QuoteIfNeeded(Text);
    }
}