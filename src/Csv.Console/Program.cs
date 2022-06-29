using Fmbm.Text;
using Fmbm.IO;

Console.WriteLine(@"Hello, World!");

var dir = DirPaths.AppRoot.CheckedPath;
var inPath = Path.Combine(dir, "cakeIn.csv");
var csvIn = new CCFile(inPath).ReadText();

var table = Csv.GetTable(csvIn);
Console.WriteLine(table[1][1].Text);
table[1][1] = new Cell(DateTime.UtcNow);
Console.WriteLine(table[1][1].Text);

