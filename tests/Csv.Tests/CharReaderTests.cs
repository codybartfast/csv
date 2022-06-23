namespace Fmbm.Csv.Tests;

public class CharReaderTests
{
    [Fact]
    public void Read_AtEndOfFile_Throws()
    {
        var text = "abc";
        var rdr = new CharReader(text);
        foreach (var expected in text)
        {
            Assert.Equal(expected, rdr.Read());
        }
        Action readLast = () => rdr.Read();
        Assert.Throws<CsvParseException>(readLast);
    }

    [Fact]
    public void TryRead_AtEndOfFile_ReturnsFalse()
    {
        var text = "abc";
        var rdr = new CharReader(text);

        foreach (var expected in text)
        {
            if (rdr.TryRead(out var c))
            {
                Assert.Equal(expected, c);
            }
            else
            {
                Assert.True(false, "TryRead should return true");
            }
        }
        if (rdr.TryRead(out var nc))
        {
            Assert.False(true, "TryRead should return false");
        }
        else
        {
            Assert.False(nc.HasValue);
        }
    }
}