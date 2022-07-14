using System.Globalization;
using System.Text.RegularExpressions;

namespace Fmbm.Text;

public partial class Cell
{
    const DateTimeStyles DateTimeStyle =
        DateTimeStyles.None |
        DateTimeStyles.AllowLeadingWhite |
        DateTimeStyles.AllowTrailingWhite;
    const NumberStyles IntStyle =
        NumberStyles.Integer |
        NumberStyles.AllowLeadingWhite |
        NumberStyles.AllowTrailingWhite |
        NumberStyles.AllowThousands;
    const NumberStyles FloatStyle =
        NumberStyles.Float |
        NumberStyles.AllowLeadingWhite |
        NumberStyles.AllowTrailingWhite |
        NumberStyles.AllowThousands;

    internal static CultureInfo DefaultCulture { get; } =
        CultureInfo.CurrentCulture;

    static public string DateTimeFormat { get; } = "yyyy-MM-dd HH:mm";

    public string Text { get; init; }
    public CultureInfo Culture { get; init; }

    public Cell(object o) : this(o, DefaultCulture) { }

    public Cell(object o, CultureInfo culture)
    {
        this.Culture = culture;

        switch (o)
        {
            case Cell cell:
                this.Text = cell.Text;
                break;
            case string text:
                this.Text = text;
                break;
            case DateTime dt:
                this.Text = dt.ToString(DateTimeFormat);
                break;
            case int n:
                this.Text = n.ToString(culture);
                break;
            case uint n:
                this.Text = n.ToString(culture);
                break;
            case long n:
                this.Text = n.ToString(culture);
                break;
            case ulong n:
                this.Text = n.ToString(culture);
                break;
            case float n:
                this.Text = n.ToString(culture);
                break;
            case double n:
                this.Text = n.ToString(culture);
                break;
            case decimal n:
                this.Text = n.ToString(culture);
                break;
            default:
                this.Text = (o?.ToString()) ?? String.Empty;
                break;
        }
    }


    public static implicit operator string(Cell cell)
    {
        return cell.Text;
    }

    public static implicit operator DateTime(Cell cell)
    {
        DateTime dt;
        if (!DateTime.TryParseExact(
            cell.Text, DateTimeFormat, null, DateTimeStyle, out dt))
        {
            dt = DateTime.Parse(cell.Text, cell.Culture, DateTimeStyle);
        }
        return dt;
    }

    public static implicit operator int(Cell cell)
    {
        return int.Parse(cell.Text, IntStyle, cell.Culture);
    }

    public static implicit operator uint(Cell cell)
    {
        return uint.Parse(cell.Text, IntStyle, cell.Culture);
    }

    public static implicit operator long(Cell cell)
    {
        return long.Parse(cell.Text, IntStyle, cell.Culture);
    }

    public static implicit operator ulong(Cell cell)
    {
        return ulong.Parse(cell.Text, IntStyle, cell.Culture);
    }

    public static implicit operator float(Cell cell)
    {
        return float.Parse(cell.Text, FloatStyle, cell.Culture);
    }

    public static implicit operator double(Cell cell)
    {
        return double.Parse(cell.Text, FloatStyle, cell.Culture);
    }

    public static implicit operator decimal(Cell cell)
    {
        return decimal.Parse(cell.Text, FloatStyle, cell.Culture);
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

    public string ToCsvText()
    {
        return QuoteIfNeeded(Text);
    }

    public override string ToString()
    {
        return Text;
    }
}
