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
    public void Parseing(){
        var date = DateTime.Parse("2002-03-04");
        Assert.Equal(date, Cell.From("04/03/02"));
    }
}