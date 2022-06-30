using System.Globalization;

using Fmbm.Text;
using Fmbm.IO;

Console.WriteLine(@"Hello, World!");

var dir = DirPaths.AppRoot.CheckedPath;
var inPath = Path.Combine(dir, "bbtIn.csv");
var outPath = Path.Combine(dir, "bbtOut.csv");
var csvTextIn = new CCFile(inPath).ReadText();

var episodes = Csv.GetItems(csvTextIn, row =>
    new Episode
    {
        NumOverall = row("No. overall"),
        NumInSeason = row("No. in season"),
        Title = row("Title"),
        OriginalAirDate = row("Original air date"),
        USViewers = row("U.S. viewers")
    }).ToArray();

var byOverall = episodes.OrderBy(ep => ep.NumOverall).ToArray();

var csvTextOut = Csv.GetText(byOverall,
    ("No.", e => e.NumOverall),
    ("Title", e => e.Title.Trim('"')),
    ("Original Air Date", e => e.OriginalAirDate),
    ("US Viewers (M)", e => Math.Round((e.USViewers / 1000000), 2)));

var csvTextOutFr = Csv.GetText(byOverall, CultureInfo.GetCultureInfo("fr-FR"),
    ("No.", e => e.NumOverall),
    ("Title", e => e.Title.Trim('"')),
    ("Original Air Date", e => e.OriginalAirDate),
    ("US Viewers (M)", e => Math.Round((e.USViewers / 1000000), 2)));

Console.WriteLine(csvTextIn);
Console.WriteLine(csvTextOut);
Console.WriteLine(csvTextOutFr);

class Episode
{
    public long NumOverall { get; set; }
    public int NumInSeason { get; set; }
    public string? Title { get; set; }
    public DateTime OriginalAirDate { get; set; }
    public decimal USViewers { get; set; }
}
