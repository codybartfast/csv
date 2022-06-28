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
    public void NastyString(){
        // more compresenisve tests in CsvParserTests
        var text = "\r \" \",\" ,, \"\"\r\"";
        Assert.Equal(text, Cell.From(text));
    }

    [Fact]
    public void SortableDateTime(){
        var date = DateTime.Parse("2002-03-04T05:06Z");
        Assert.Equal(date, Cell.From("2002-03-04 5:06"));
    }

    [Fact]
    public void SortableDate(){
        var date = DateTime.Parse("2002-03-04");
        Assert.Equal(date, Cell.From("2002-03-04"));
    }

    [Fact]
    public void StupidAmericanStyleDate(){
        var date = DateTime.Parse("2002-03-04");
        Assert.Equal(date, Cell.From("03/04/02"));
    }    
}