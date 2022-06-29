namespace Fmbm.Text.Tests;

public class CellTests
{
    [Fact]
    public void RoundTripReturnsSame()
    {
        var s = " 123 ";
        Assert.Equal(s, new Cell(s));

        var dt = DateTime.Parse("2022-06-27T16:04Z");
        Assert.Equal(dt, new Cell(dt));

        int i = 123;
        Assert.Equal(i, new Cell(i));

        uint ui = 123;
        Assert.Equal(ui, new Cell(ui));

        long l = 123;
        Assert.Equal(l, new Cell(l));

        ulong ul = 123;
        Assert.Equal(ul, new Cell(ul));

        float f = 1234.5678F;
        Assert.Equal(f, new Cell(f));

        double d = 1234.5678;
        Assert.Equal(d, new Cell(d));

        decimal m = 1234.5678m;
        Assert.Equal(m, new Cell(m));
    }


    [Fact]
    public void NastyString()
    {
        // more compresenisve tests in CsvParserTests
        var text = "\r \" \",\" ,, \"\"\r\"";
        Assert.Equal(text, new Cell(text));
    }

    [Fact]
    public void SortableDateTime()
    {
        var date = DateTime.Parse("2002-03-04T05:06Z");
        Assert.Equal(date, new Cell("2002-03-04 5:06"));
    }

    [Fact]
    public void SortableDate()
    {
        var date = DateTime.Parse("2002-03-04");
        Assert.Equal(date, new Cell("2002-03-04"));
    }

    [Fact]
    public void DaftAmericanStyleDate()
    {
        var date = DateTime.Parse("2002-03-04");
        Assert.Equal(date, new Cell("03/04/02"));
    }

    [Fact]
    public void Integers()
    {
        int i = 1234;
        Assert.Equal(i, new Cell("1234"));
        Assert.Equal(i, new Cell("001234"));
        Assert.Equal(i, new Cell("1,234"));
        Assert.Equal(i, new Cell(" 1,234 "));
        Assert.Equal(i, new Cell(" 1234 "));
        Assert.Equal(-i, new Cell(" -1,234 "));

        uint u = 1234u;
        Assert.Equal(u, new Cell("1234"));
        Assert.Equal(u, new Cell("001234"));
        Assert.Equal(u, new Cell("1,234"));
        Assert.Equal(u, new Cell(" 1,234 "));
        Assert.Equal(u, new Cell(" 1234 "));
        Action uNeg = () => { var _ = (uint)new Cell(" -1,234 "); };
        Assert.Throws<OverflowException>(uNeg);

        long l = 1234;
        Assert.Equal(l, new Cell("1234"));
        Assert.Equal(l, new Cell("001234"));
        Assert.Equal(l, new Cell("1,234"));
        Assert.Equal(l, new Cell(" 1,234 "));
        Assert.Equal(l, new Cell(" 1234 "));
        Assert.Equal(-l, new Cell(" -1,234 "));

        ulong ul = 1234;
        Assert.Equal(ul, new Cell("1234"));
        Assert.Equal(ul, new Cell("001234"));
        Assert.Equal(ul, new Cell("1,234"));
        Assert.Equal(ul, new Cell(" 1,234 "));
        Assert.Equal(ul, new Cell(" 1234 "));
        Action ulNeg = () => { var _ = (ulong)new Cell(" -1,234 "); };
        Assert.Throws<OverflowException>(ulNeg);
    }

    [Fact]
    public void NonInteger()
    {
        float f = 1234.567f;
        Assert.Equal(f, new Cell("1234.567"));
        Assert.Equal(f, new Cell("001234.56700"));
        Assert.Equal(f, new Cell("1,234.567"));
        Assert.Equal(f, new Cell(" 1,234.567 "));
        Assert.Equal(f, new Cell(" 1234.567 "));
        Assert.Equal(-f, new Cell(" -1,234.567 "));
        Assert.Equal(-f, new Cell(" -01.2345670E+3 "));

        double d = 1234.567;
        Assert.Equal(d, new Cell("1234.567"));
        Assert.Equal(d, new Cell("001234.56700"));
        Assert.Equal(d, new Cell("1,234.567"));
        Assert.Equal(d, new Cell(" 1,234.567 "));
        Assert.Equal(d, new Cell(" 1234.567 "));
        Assert.Equal(-d, new Cell(" -1,234.567 "));
        Assert.Equal(-d, new Cell(" -01.2345670E+3 "));

        decimal m = 1234.567m;
        Assert.Equal(m, new Cell("1234.567"));
        Assert.Equal(m, new Cell("001234.56700"));
        Assert.Equal(m, new Cell("1,234.567"));
        Assert.Equal(m, new Cell(" 1,234.567 "));
        Assert.Equal(m, new Cell(" 1234.567 "));
        Assert.Equal(-m, new Cell(" -1,234.567 "));
        Assert.Equal(-m, new Cell(" -01.2345670E+3 "));
    }

    [Fact]
    public void FromAny_UnknownType_ToString()
    {
        byte b = 255;
        Assert.Equal(b, byte.Parse(new Cell(b)));
    }
}