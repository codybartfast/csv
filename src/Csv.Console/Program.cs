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
        USViewersMillions = row("U.S. viewers (millions)")
    }).ToArray();

var mostViews = episodes.MaxBy(e => e.USViewersMillions);
var firstAired = episodes.MinBy(e => e.OriginalAirDate);
Console.WriteLine($"Most viewed: {mostViews!.Title}");
Console.WriteLine($"First Aired: {firstAired!.Title}");

episodes = episodes.OrderByDescending(ep => ep.USViewersMillions).ToArray();

// var csvTextOut = Csv.GetText(episodes,
//     ("Id", e => e.NumOverall),
//     ("Title", e => e.Title),
//     ("Original Air Date", e => e.OriginalAirDate),
//     ("US Viewers (M)", e => e.USViewersMillions));

// new CCFile(outPath).WriteText(csvTextOut);

// Console.WriteLine(csvTextIn);
// Console.WriteLine(csvTextOut);

class Episode
{
    public long NumOverall { get; set; }
    public int NumInSeason { get; set; }
    public string? Title { get; set; }

    public DateTime OriginalAirDate { get; set; }
    public decimal USViewersMillions { get; set; }
}
