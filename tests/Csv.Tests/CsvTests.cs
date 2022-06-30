using System.Globalization;
using System.Text.RegularExpressions;

namespace Fmbm.Text.Tests;

public class CsvTests
{
    CultureInfo usClt = CultureInfo.GetCultureInfo("en-US");
    CultureInfo frClt = CultureInfo.GetCultureInfo("fr-FR");

    [Fact]
    public void GivenCulture_UsedToWriteText()
    {
        var nums = new decimal[] { 1.234m };

        var usText = Csv.GetText(nums, usClt, ("Number", n => n));
        var usValText = Regex.Split(usText, "\r?\n")[1];
        Assert.Equal("1.234", usValText);

        var frText = Csv.GetText(nums, frClt, ("Number", n => n));
        var frValText = Regex.Split(frText, "\r?\n")[1];
        Assert.Equal("\"1,234\"", frValText);
    }

    [Fact]
    public void GivenCulture_UsedToParseText()
    {
        var csvText = "Number\r\n\"123,456\"";

        var usNums = Csv.GetItems(csvText, usClt, row => row("Number"));
        Assert.Equal<decimal>(123456m, usNums.First());

        var frNums = Csv.GetItems(csvText, frClt, row => row("Number"));
        Assert.Equal<decimal>(123.456m, frNums.First());
    }
}
