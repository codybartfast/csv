namespace Fmbm.Csv;

public class CsvException : Exception
{
    public CsvException(string msg) : base(msg) { }
}

public class CsvParseException : CsvException
{
    public CsvParseException(string msg) : base(msg) { }
}