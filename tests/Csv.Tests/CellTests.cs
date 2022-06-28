namespace Fmbm.Text.Tests;

public class CellTests
{
    [Fact]
    public void RoundTripReturnsSame()
    {
        var s = " 123 ";
        Assert.Equal(s, Cell.From(s));

        var dt = DateTime.Parse("2022-06-27T16:04Z");
        Assert.Equal(dt, Cell.From(dt));

        int i = 123;
        Assert.Equal(i, Cell.From(i));

        uint ui = 123;
        Assert.Equal(ui, Cell.From(ui));

        long l = 123;
        Assert.Equal(l, Cell.From(l));

        ulong ul = 123;
        Assert.Equal(ul, Cell.From(ul));

        float f = 1234.5678F;
        Assert.Equal(f, Cell.From(f));

        double d = 1234.5678;
        Assert.Equal(d, Cell.From(d));

        decimal m = 1234.5678m;
        Assert.Equal(m, Cell.From(m));
    }


    [Fact]
    public void NastyString()
    {
        // more compresenisve tests in CsvParserTests
        var text = "\r \" \",\" ,, \"\"\r\"";
        Assert.Equal(text, Cell.From(text));
    }

    [Fact]
    public void SortableDateTime()
    {
        var date = DateTime.Parse("2002-03-04T05:06Z");
        Assert.Equal(date, Cell.From("2002-03-04 5:06"));
    }

    [Fact]
    public void SortableDate()
    {
        var date = DateTime.Parse("2002-03-04");
        Assert.Equal(date, Cell.From("2002-03-04"));
    }

    [Fact]
    public void StupidAmericanStyleDate()
    {
        var date = DateTime.Parse("2002-03-04");
        Assert.Equal(date, Cell.From("03/04/02"));
    }

    [Fact]
    public void Integers()
    {
        int i = 1234;
        Assert.Equal(i, Cell.From("1234"));
        Assert.Equal(i, Cell.From("001234"));
        Assert.Equal(i, Cell.From("1,234"));
        Assert.Equal(i, Cell.From(" 1,234 "));
        Assert.Equal(i, Cell.From(" 1234 "));
        Assert.Equal(-i, Cell.From(" -1,234 "));

        uint u = 1234u;
        Assert.Equal(u, Cell.From("1234"));
        Assert.Equal(u, Cell.From("001234"));
        Assert.Equal(u, Cell.From("1,234"));
        Assert.Equal(u, Cell.From(" 1,234 "));
        Assert.Equal(u, Cell.From(" 1234 "));
        Action uNeg = () => { var _ = (uint)Cell.From(" -1,234 "); };
        Assert.Throws<OverflowException>(uNeg);

        long l = 1234;
        Assert.Equal(l, Cell.From("1234"));
        Assert.Equal(l, Cell.From("001234"));
        Assert.Equal(l, Cell.From("1,234"));
        Assert.Equal(l, Cell.From(" 1,234 "));
        Assert.Equal(l, Cell.From(" 1234 "));
        Assert.Equal(-l, Cell.From(" -1,234 "));

        ulong ul = 1234;
        Assert.Equal(ul, Cell.From("1234"));
        Assert.Equal(ul, Cell.From("001234"));
        Assert.Equal(ul, Cell.From("1,234"));
        Assert.Equal(ul, Cell.From(" 1,234 "));
        Assert.Equal(ul, Cell.From(" 1234 "));
        Action ulNeg = () => { var _ = (ulong)Cell.From(" -1,234 "); };
        Assert.Throws<OverflowException>(ulNeg);
    }

    [Fact]
    public void NonInteger()
    {
        float f = 1234.567f;
        Assert.Equal(f, Cell.From("1234.567"));
        Assert.Equal(f, Cell.From("001234.56700"));
        Assert.Equal(f, Cell.From("1,234.567"));
        Assert.Equal(f, Cell.From(" 1,234.567 "));
        Assert.Equal(f, Cell.From(" 1234.567 "));
        Assert.Equal(-f, Cell.From(" -1,234.567 "));
        Assert.Equal(-f, Cell.From(" -01.2345670E+3 "));

        double d = 1234.567;
        Assert.Equal(d, Cell.From("1234.567"));
        Assert.Equal(d, Cell.From("001234.56700"));
        Assert.Equal(d, Cell.From("1,234.567"));
        Assert.Equal(d, Cell.From(" 1,234.567 "));
        Assert.Equal(d, Cell.From(" 1234.567 "));
        Assert.Equal(-d, Cell.From(" -1,234.567 "));
        Assert.Equal(-d, Cell.From(" -01.2345670E+3 "));

        decimal m = 1234.567m;
        Assert.Equal(m, Cell.From("1234.567"));
        Assert.Equal(m, Cell.From("001234.56700"));
        Assert.Equal(m, Cell.From("1,234.567"));
        Assert.Equal(m, Cell.From(" 1,234.567 "));
        Assert.Equal(m, Cell.From(" 1234.567 "));
        Assert.Equal(-m, Cell.From(" -1,234.567 "));
        Assert.Equal(-m, Cell.From(" -01.2345670E+3 "));
    }
}