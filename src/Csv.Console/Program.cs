using Fmbm.Text;

var csvTextIn = @"Title,No. overall,No. in season,Original air date,Prod. code,U.S. viewers
The Bad Fish Paradigm,18, 1 ,""September 22, 2008"",3T7351,9360364
The Barbarian Sublimation,20, 3 ,""October 6, 2008"",3T7353,9329673
The Codpiece Topology,19, 2 ,""September 29, 2008"",3T7352,8758200
The Cooper-Nowitzki Theorem,23, 6 ,""November 3, 2008"",3T7356,9670118
The Euclid Alternative,22, 5 ,""October 20, 2008"",3T7355,9280649
The Griffin Equivalency,21, 4 ,""October 13, 2008"",3T7354,9356497";

Episode[] episodes = Csv.GetItems(csvTextIn, row =>
    new Episode
    {
        NumOverall = row("No. overall"),
        NumInSeason = row("No. in season"),
        Title = row("Title"),
        OriginalAirDate = row("Original air date"),
        USViewers = row("U.S. viewers")
    }).ToArray();

string csvTextOut = Csv.GetText(episodes,
    ("No. Overall", ep => ep.NumOverall),
    ("No. In Season", ep => ep.NumInSeason),
    ("Title", ep => ep.Title),
    ("Original Air Date", ep => ep.OriginalAirDate),
    ("US Viewers", ep => ep.USViewers));

Console.WriteLine(csvTextOut);

class Episode
{
    public long NumOverall { get; set; }
    public int NumInSeason { get; set; }
    public string? Title { get; set; }
    public DateTime OriginalAirDate { get; set; }
    public decimal USViewers { get; set; }
}
