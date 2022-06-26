namespace Fmbm.Text;
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

    public char Peek()
    {
        var i = sr.Peek();
        if (i == -1)
        {
            throw new CsvParseException("Tried to Peek at End of File");
        }
        return (char)i;
    }

    public bool AtEnd => sr.Peek() == -1;
}
