namespace Fmbm.Text;
internal class CharReader
{
    readonly string text;
    readonly int length;
    int pos = 0;

    public CharReader(string text)
    {
        this.text = text;
        this.length = text.Length;
    }

    public char Peek()
    {
        if (pos >= length)
        {
            throw new CsvParseException(
                "Tried to read/peek beyond end of text");
        }
        return text[pos];
    }

    public char Read()
    {
        var c = Peek();
        pos++;
        return c;
    }

    public bool AtEnd => pos >= length;
}
