namespace Fmbm.Text.Tests;

public class CharReaderTests
{
    [Fact]
    public void Read_AtEndOfFile_Throws()
    {
        var text = "abc";
        var rdr = new CharReader(text);
        foreach (var expected in text)
        {
            Assert.False(rdr.AtEnd);
            Assert.Equal(expected, rdr.Read());
        }
        Assert.True(rdr.AtEnd);
        Action readLast = () => rdr.Read();
        Assert.Throws<CsvParseException>(readLast);
    }

    [Fact]
    public void Peek()
    {
        Assert.True(false);
    }
}