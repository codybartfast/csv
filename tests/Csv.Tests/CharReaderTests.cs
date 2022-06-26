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
    public void Peak_AtEndOfFile_Throws()
    {
        var text = "abc";
        var rdr = new CharReader(text);
        for (var i = 0; i < text.Length; i++)
        {
            Assert.False(rdr.AtEnd);
            Assert.Equal(text[i], rdr.Peek());
            Assert.Equal(text[i], rdr.Peek());
            rdr.Read();
        }
        Assert.True(rdr.AtEnd);
        Action peekLast = () => rdr.Read();
        Assert.Throws<CsvParseException>(peekLast);
    }
}