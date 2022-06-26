namespace Fmbm.Text.Tests;

public class TableReaderTests
{
    [Fact]
    public void EmptyString_SingleEmptyCell()
    {
        var table = TableReader.GetTable("");
        Assert.Equal(1, table.Length);
        var row = table[0];
        Assert.Equal(1, row.Length);
        var cell = row[0];
        Assert.Equal(string.Empty, cell);
    }

    [Fact]
    public void PlainString_SingleMatchingCell()
    {
        var text = "The quick brown fox.";
        var table = TableReader.GetTable(text);
        Assert.Equal(1, table.Length);
        var row = table[0];
        Assert.Equal(1, row.Length);
        var cell = row[0];
        Assert.Equal(text, cell);
    }

    [Fact]
    public void SingleRow()
    {
        var text = "The, quick, brown, fox.";
        var table = TableReader.GetTable(text);
        Assert.Equal(1, table.Length);
        var row = table[0];
        Assert.Equal(4, row.Length);
        Assert.Equal("The", row[0]);
        Assert.Equal(" quick", row[1]);
        Assert.Equal(" brown", row[2]);
        Assert.Equal(" fox.", row[3]);
    }


    [Fact]
    public void SingleCol()
    {
        var text = "The\n quick\n brown\n fox.";
        var table = TableReader.GetTable(text);
        Assert.Equal(4, table.Length);
        foreach (var row in table.Rows)
        {
            Assert.Equal(1, row.Length);
        }
        Assert.Equal("The", table[0][0]);
        Assert.Equal(" quick", table[1][0]);
        Assert.Equal(" brown", table[2][0]);
        Assert.Equal(" fox.", table[3][0]);
    }

    [Fact]
    public void EmptyCells()
    {
        var text = ",,,\n,,,\n,,,";
        var table = TableReader.GetTable(text);
        Assert.Equal(3, table.Length);
        foreach (var row in table.Rows)
        {
            Assert.Equal(4, row.Length);
            foreach (var cell in row.Cells)
            {
                Assert.Equal(string.Empty, cell);
            }
        }
    }

    [Fact]
    public void BlankCells()
    {
        var text = "   ,   ,   ,   \n   ,   ,   ,   \n   ,   ,   ,   ";
        var table = TableReader.GetTable(text);
        Assert.Equal(3, table.Length);
        foreach (var row in table.Rows)
        {
            Assert.Equal(4, row.Length);
            foreach (var cell in row.Cells)
            {
                Assert.Equal("   ", cell);
            }
        }
    }

    [Fact]
    public void FinalNewLine()
    {
        var text = "   ,   ,   ,   \n   ,   ,   ,   \n   ,   ,   ,   \n";
        var table = TableReader.GetTable(text);
        Assert.Equal(3, table.Length);
        foreach (var row in table.Rows)
        {
            Assert.Equal(4, row.Length);
        }
        Assert.Equal("   ", table.Rows.Last().Cells.Last());
    }

    [Fact]
    public void CarriageReturnNL()
    {
        var text = "   ,   ,   ,   \r\n   ,   ,   ,   \r\n   ,   ,   ,   \r\n";
        var table = TableReader.GetTable(text);
        Assert.Equal(3, table.Length);
        foreach (var row in table.Rows)
        {
            Assert.Equal(4, row.Length);
        }
        Assert.Equal("   ", table.Rows.Last().Cells.Last());
    }

    [Fact]
    public void OtherCarriageReturn()
    {
        var text =
            "\r \r,\r \r,\r \r,\r \r\r\n"
            + "\r \r,\r \r,\r \r,\r \r\r\n"
            + "\r \r,\r \r,\r \r,\r \r";
        var table = TableReader.GetTable(text);
        Assert.Equal(3, table.Length);
        foreach (var row in table.Rows)
        {
            Assert.Equal(4, row.Length);
        }
        Assert.Equal("\r \r", table.Rows.Last().Cells.Last());
    }

    [Fact]
    public void CorrectOrder()
    {
        var text = "00,01,02,03\n10,11,12,13\n20,21,22,23\n";
        var table = TableReader.GetTable(text);
        Assert.Equal(3, table.Length);
        foreach (var row in table.Rows)
        {
            Assert.Equal(4, row.Length);
        }
        for (var r = 0; r < table.Length; r++)
        {
            for (var c = 0; c < table[0].Length; c++)
            {
                Assert.Equal($"{r}{c}", table[r][c]);
            }
        }
    }
}
