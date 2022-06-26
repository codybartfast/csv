namespace Fmbm.Text;

internal class CsvParser
{
    const char doubleQuote = '"';

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
        var chars = new List<char>();
        char c;

        while (!reader.AtEnd)
        {
            switch (c = reader.Read())
            {
                case ',':
                    endOfRow = false;
                    return Cell();
                case '\r':
                    if (!reader.AtEnd && reader.Peek() == '\n')
                    {
                        reader.Read();
                        endOfRow = true;
                        return Cell();
                    }
                    chars.Add(c);
                    break;
                case '\n':
                    endOfRow = true;
                    return Cell();
                case '"':
                    // Check there weren't invalid characters before quoted
                    foreach (var leadingChar in chars)
                    {
                        if (!Char.IsWhiteSpace(leadingChar))
                        {
                            throw new CsvParseException(
                                $"Non-whitespace char '{leadingChar}' found before opening double quote.");
                        }
                    }
                    // Read quoted
                    chars = ReadQuoted(reader);
                    // Ignore insignificant whitespace after quoted
                    char p;
                    while (!reader.AtEnd
                        && Char.IsWhiteSpace(p = reader.Peek()) && p != '\n')
                    {
                        reader.Read();
                    }
                    // Check for invalid characters after quoted
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
                    chars.Add(c);
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

    static List<char> ReadQuoted(CharReader reader)
    {
        var chars = new List<char>();
        char c;

        while (true)
        {
            switch (c = reader.Read())
            {
                case doubleQuote:
                    if (!reader.AtEnd && reader.Peek() == doubleQuote)
                    {
                        reader.Read();
                        chars.Add(doubleQuote);
                        break;
                    }
                    else
                    {
                        return chars;
                    }
                default:
                    chars.Add(c);
                    break;
            }
        }
    }
}
