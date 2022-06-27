using Fmbm.Text;
using Fmbm.IO;

Console.WriteLine(@"

More numeric conversions
Sample (external) converter.

Hello, World!");

var dir = DirPaths.AppRoot.CheckedPath;
var inPath = Path.Combine(dir, "cakeIn.csv");
var outPath = Path.Combine(dir, "cakeOut.csv");
var csvIn = new CCFile(inPath).ReadText();

var cakes = Csv.GetItems(csvIn, row =>
    new Cake(
        row("Name"),
        row("Some Text"),
        row("Date"),
        row("Number")))
    .ToArray();

Console.WriteLine(cakes[0]);
Console.WriteLine(cakes[1]);

var csvOut = Csv.GetText(cakes,
    ("Date", c => c.Added),
    ("Number", c => c.Serves),
    ("Some Text", c => c.Recipe),
    ("Name", c => c.Name)
);

new CCFile(outPath).WriteText(csvOut);


public class Cake
{
    public Cake(string name, string recipe, DateTime added, int serves)
    {
        this.Name = name;
        this.Recipe = recipe;
        this.Added = added;
        this.Serves = serves;
    }

    public string Name { get; init; }
    public string Recipe { get; init; }
    public DateTime Added { get; init; }
    public int Serves { get; init; }

    public override string ToString()
    {
        return $"{Name} cake has served {Serves} people since {Added} it needs: {Recipe}";
    }
}