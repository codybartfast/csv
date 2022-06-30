using System.Globalization;
using System.Text.RegularExpressions;

namespace Fmbm.Text;

public partial class Cell
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

    public Cell(int n)
    {
        this.Text = n.ToString(culture);
    }

    public Cell(uint n)
    {
        this.Text = n.ToString(culture);
    }

    public Cell(long n)
    {
        this.Text = n.ToString(culture);
    }

    public Cell(ulong n)
    {
        this.Text = n.ToString(culture);
    }

    public Cell(float n)
    {
        this.Text = n.ToString(culture);
    }

    public Cell(double n)
    {
        this.Text = n.ToString(culture);
    }

    public Cell(decimal n)
    {
        this.Text = n.ToString(culture);
    }

    public Cell(object o)
    {
        switch (o)
        {
            case string text:
                this.Text = text;
                break;
            case DateTime dt:
                this.Text = dt.ToString("yyyy-MM-dd HH:mm");
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

    // public static implicit operator Cell(string text)
    // {
    //     return new Cell(text);
    // }

    public static implicit operator string(Cell cell)
    {
        return cell.Text;
    }

    public static implicit operator DateTime(Cell cell)
    {
        return DateTime.Parse(cell.Text, culture);
    }

    // public static implicit operator Cell(DateTime dt)
    // {
    //     return new Cell(dt);
    // }

    public static implicit operator int(Cell cell)
    {
        return int.Parse(cell.Text, IntStyles, culture);
    }

    // public static implicit operator Cell(int n)
    // {
    //     return new Cell(n);
    // }

    public static implicit operator uint(Cell cell)
    {
        return uint.Parse(cell.Text, IntStyles, culture);
    }

    // public static implicit operator Cell(uint n)
    // {
    //     return new Cell(n);
    // }

    public static implicit operator long(Cell cell)
    {
        return long.Parse(cell.Text, IntStyles, culture);
    }

    // public static implicit operator Cell(long n)
    // {
    //     return new Cell(n);
    // }

    public static implicit operator ulong(Cell cell)
    {
        return ulong.Parse(cell.Text, IntStyles, culture);
    }

    // public static implicit operator Cell(ulong n)
    // {
    //     return new Cell(n);
    // }

    public static implicit operator float(Cell cell)
    {
        return float.Parse(cell.Text, FloatStyles, culture);
    }

    // public static implicit operator Cell(float n)
    // {
    //     return new Cell(n);
    // }

    public static implicit operator double(Cell cell)
    {
        return double.Parse(cell.Text, FloatStyles, culture);
    }

    // public static implicit operator Cell(double n)
    // {
    //     return new Cell(n);
    // }

    public static implicit operator decimal(Cell cell)
    {
        return decimal.Parse(cell.Text, FloatStyles, culture);
    }

    // public static implicit operator Cell(decimal n)
    // {
    //     return new Cell(n);
    // }

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
