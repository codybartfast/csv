namespace Fmbm.Text.Tests;

public class CsvVerifyTableTests
{
    [Fact]
    public void MustHaveHeaderRow()
    {
        var table = new Table(new Row[0]);
        var verify = () => Csv.VerifyTable(table);
        Assert.Throws<CsvException>(verify);
    }

    [Fact]
    public void MustNotDuplicateHeaders()
    {
        var table = CsvParser.GetTable("a,b,A");
        var verify = () => Csv.VerifyTable(table);
        Assert.Throws<CsvException>(verify);
    }

    [Fact]
    public void AllRowsMustHaveSameLength()
    {
        var csv = "H1,H2,H3,H4\na,b,c,d\n1,2,3\nA,B,C,D";
        var table = CsvParser.GetTable(csv);
        var verify = () => Csv.VerifyTable(table);
        Assert.Throws<CsvException>(verify);
    }
}
