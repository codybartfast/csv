namespace Fmbm.Text.Tests;

public class CsvVerifyTableTests
{
    [Fact]
    public void MustHaveHeaderRow()
    {
        var table = new Table(new Row[0]);
        var verify = () => Csv.VerifyItemTable(table);
        Assert.Throws<CsvException>(verify);
    }

    [Fact]
    public void MustNotDuplicateHeaders()
    {
        var table = TextParser.GetTable("a,b,A");
        var verify = () => Csv.VerifyItemTable(table);
        Assert.Throws<CsvException>(verify);
    }

    [Fact]
    public void AllRowsMustHaveSameLength()
    {
        var csv = "H1,H2,H3,H4\na,b,c,d\n1,2,3\nA,B,C,D";
        var table = TextParser.GetTable(csv);
        var verify = () => Csv.VerifyItemTable(table);
        Assert.Throws<CsvException>(verify);
    }
}
