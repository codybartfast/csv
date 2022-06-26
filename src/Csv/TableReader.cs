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

    static string ReadQuoted(CharReader reader)
    {
        var chars = new LinkedList<char>();
        char c;
        bool pendingDoubleQuote = false;
        while (!reader.AtEnd)
        {
            switch (c = reader.Read())
            {
                case '"':
                    if (pendingDoubleQuote)
                    {
                        pendingDoubleQuote = false;
                        chars.AddLast(c);
                        break;
                    }
                    else
                    {
                        /////////// Wrong, need to set as pending and peek.
                        return Text();
                    }
                default:
                    chars.AddLast(c);
                    break;
            }
        }
        if (pendingDoubleQuote)
        {
            return Text();
        }
        else
        {
            throw new Exception();
        }

        string Text()
        {
            var text = new String(chars.ToArray());
            return new Cell(text);

        }
    }
