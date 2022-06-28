namespace Fmbm.Text.Tests;

public class TextParserTests
{
    [Fact]
    public void EmptyString_SingleEmptyCell()
    {
        var table = TextParser.GetTable("");
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
        var table = TextParser.GetTable(text);
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
        var table = TextParser.GetTable(text);
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
        var table = TextParser.GetTable(text);
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
        var table = TextParser.GetTable(text);
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
        var table = TextParser.GetTable(text);
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
        var table = TextParser.GetTable(text);
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
        var table = TextParser.GetTable(text);
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
        var table = TextParser.GetTable(text);
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
        var table = TextParser.GetTable(text);
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

    [Fact]
    public void SimpleQuotes()
    {
        var text =
            "\"00\",\"01\",\"02\",\"03\"\n"
            + "\"10\",\"11\",\"12\",\"13\"\n"
            + "\"20\",\"21\",\"22\",\"23\"\n";
        var table = TextParser.GetTable(text);
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

    [Fact]
    public void QuotedTable()
    {
        var innerText = Cell.QuoteIfNeeded(
            "\"00\",\"01\",\"02\",\"03\"\n"
            + "\"10\",\":-)\",\"12\",\"13\"\n"
            + "\"20\",\"21\",\"22\",\"23\"\n");
        var middleText = Cell.QuoteIfNeeded(
            "\"00\",\"01\",\"02\",\"03\"\r\n"
            + $"\"10\",{innerText},\"12\",\"13\"\r\n"
            + "\"20\",\"21\",\"22\",\"23\"\r\n");
        var outerText =
            "\"00\",\"01\",\"02\",\"03\"\n"
            + $"\"10\",{middleText},\"12\",\"13\"\n"
            + "\"20\",\"21\",\"22\",\"23\"\n";
        var outerTable = TextParser.GetTable(outerText);
        Check(outerTable);
        var middleTable = TextParser.GetTable(outerTable[1][1]);
        Check(middleTable);
        var innerTable = TextParser.GetTable(middleTable[1][1]);
        Check(innerTable, ":-)");

        void Check(Table table, string? value1_1 = null)
        {
            Assert.Equal(3, table.Length);
            foreach (var row in table.Rows)
            {
                Assert.Equal(4, row.Length);
            }
            for (var r = 0; r < table.Length; r++)
            {
                for (var c = 0; c < table[0].Length; c++)
                {
                    if (c == 1 && r == 1)
                    {
                        if(value1_1 != null){
                            Assert.Equal(value1_1, table[1][1]);
                        }
                    }
                    else
                    {
                        Assert.Equal($"{r}{c}", table[r][c]);
                    }
                }
            }
        }
    }

    [Fact]
    public void AllowWhitespaceAroundQuoted()
    {
        var text =
            " \"00\"  ,  \"01\"  ,  \"02\"  ,  \"03\"  \n"
            + " \"10\"  ,   \"11\"  ,   \"12\"  ,   \"13\"  \r\n"
            + "     \"20\"  ,  \r  \"21\"  ,  \r\"22\"   ,  \"23\"\r   \n";
        var table = TextParser.GetTable(text);
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

    [Fact]
    public void ThrowOnBadQuoted()
    {
        var bText = "The,  X  \" quick\"  , brown, fox.";
        Action charBeforeQuoted = () => TextParser.GetTable(bText);
        Assert.Throws<CsvParseException>(charBeforeQuoted);

        var aText = "The,  \" quick\"  Y  , brown, fox.";
        Action charAfterQuoted = () => TextParser.GetTable(aText);
        Assert.Throws<CsvParseException>(charAfterQuoted);
    }
}
