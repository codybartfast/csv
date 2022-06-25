namespace Fmbm.Text.Tests;

public class CsvReaderTests
{
    [Fact]
    public void EmptyString_SingleEmptyCell()
    {
        var table = TableReader.GetTable("");
        Assert.Equal(1, table.Rows.Length);
        var row = table.Rows[0];
        Assert.Equal(1, row.Cells.Length);
        var cell = row.Cells[0];
        Assert.Equal(string.Empty, cell.Text);
    }
}
