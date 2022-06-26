namespace Fmbm.Text;

internal class TableReader
{
    public static Table GetTable(string text)
    {
        var reader = new CharReader(text);
        var rows = ReadRows(reader);
        return new Table(rows);
    }

    static IEnumerable<Row> ReadRows(CharReader reader)
    {
        do
        {
            var cells = ReadCells(reader);
            yield return new Row(cells);
        } while (!reader.AtEnd);
    }

    static IEnumerable<Cell> ReadCells(CharReader reader)
    {
        bool endOfRow;
        do
        {
            yield return ReadCell(reader, out endOfRow);
        } while (!endOfRow);
    }

    static Cell ReadCell(CharReader reader, out bool endOfRow)
    {
        var chars = new LinkedList<char>();
        string? quotedText = null;
        char c;

        while (!reader.AtEnd)
        {
            switch (c = reader.Read())
            {
                case ',':
                    endOfRow = false;
                    return Cell();
                case '\n':
                    if (chars.Any() && chars.Last() == '\r')
                    {
                        chars.RemoveLast();
                    }
                    endOfRow = true;
                    return Cell();
                case '"':
                    foreach (var leadingChar in chars)
                    {
                        if (!Char.IsWhiteSpace(leadingChar))
                        {
                            throw new CsvParseException(
                                $"Non-whitespace char '{leadingChar}' found before opening double quote.");
                        }
                    }
                    chars = ReadQuoted(reader);
                    char p;
                    if (reader.AtEnd || (p = reader.Peek()) == ',' || p == '\n')
                    {
                        break;
                    }
                    else
                    {
                        throw new CsvParseException(
                            $"Non-whitespace char '{p}' found after closing double quote.");
                    }
                default:
                    chars.AddLast(c);
                    break;
            }
        }
        endOfRow = true;
        return Cell();

        Cell Cell()
        {
            var text = new String(chars.ToArray());
            return new Cell(text);
        }
    }

    static LinkedList<char> ReadQuoted(CharReader reader)
    {
        var chars = new LinkedList<char>();
        char c;

        while (true)
        {
            switch (c = reader.Read())
            {
                case '"':
                    if (!reader.AtEnd && reader.Peek() == c)
                    {
                        reader.Read();
                        chars.AddLast(c);
                        break;
                    }
                    else
                    {
                        char p;
                        while (!reader.AtEnd
                            && Char.IsWhiteSpace(p = reader.Peek()) && p != '\n')
                        {
                            reader.Read();
                        }
                        return chars;
                    }
                default:
                    chars.AddLast(c);
                    break;
            }
        }
    }
}
