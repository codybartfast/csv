namespace Fmbm.Csv;
internal class CharReader
{
    readonly StringReader sr;

    public CharReader(string text)
    {
        this.sr = new StringReader(text);
    }

    public char Read()
    {
        var i = sr.Read();
        if (i == -1)
        {
            throw new CsvParseException("Unexpected End Of File");
        }
        return (char)i;
    }

    public bool TryRead(out char? chr)
    {
        var i = sr.Read();
        if (i == -1)
        {
            chr = null;
            return false;
        }
        chr = (char)i;
        return true;
    }

    public bool AtEnd => sr.Peek() == -1;
}
