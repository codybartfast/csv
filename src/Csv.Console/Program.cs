using System.Globalization;

using Fmbm.Text;
using Fmbm.IO;

Console.WriteLine(@"Hello, World!");

var dir = DirPaths.AppRoot.CheckedPath;
var inPath = Path.Combine(dir, "bbtIn.csv");
var outPath = Path.Combine(dir, "bbtOut.csv");
var csvTextIn = new CCFile(inPath).ReadText();

Episode[] episodes = Csv.GetItems(csvTextIn, row =>
    new Episode
    {
        NumOverall = row("No. overall"),
        NumInSeason = row("No. in season"),
        Title = row("Title").Text,
        OriginalAirDate = row("Original air date"),
        USViewers = row("U.S. viewers")
    }).ToArray();

var byOverall = episodes.OrderBy(ep => ep.NumOverall).ToArray();

string csvTextOut = Csv.GetText(episodes, CultureInfo.InvariantCulture,
    ("No. Overall", ep => ep.NumOverall),
    ("No. In Season", ep => ep.NumInSeason),
    ("Title", ep => ep.Title),
    ("Original Air Date", ep => ep.OriginalAirDate),
    ("US Viewers", ep => ep.USViewers));

var csvTextOutFr = Csv.GetText(byOverall, CultureInfo.GetCultureInfo("fr-FR"),
    ("No.", e => e.NumOverall),
    ("Title", e => e.Title.Trim('"')),
    ("Original Air Date", e => e.OriginalAirDate),
    ("US Viewers (M)", e => Math.Round((e.USViewers / 1000000), 2)));

new CCFile(outPath).WriteText(csvTextOut);

Console.WriteLine(csvTextIn);
Console.WriteLine(csvTextOut);
// Console.WriteLine(csvTextOutFr);

var p = 0.4567m;
var t = p.ToString("0.0%");
Console.WriteLine(t);
var P = decimal.Parse(t.TrimEnd('%'));
Console.WriteLine(P);

var _ = CultureInfo.InvariantCulture;

class Episode
{
    public long NumOverall { get; set; }
    public int NumInSeason { get; set; }
    public string? Title { get; set; }
    public DateTime OriginalAirDate { get; set; }
    public decimal USViewers { get; set; }
}
