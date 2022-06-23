using System.Text;
using Fmbm.Csv;

// See https://aka.ms/new-console-template for more information
Console.WriteLine("Hello, World!");

var cake = new Cake ("Fruit", "Bake");
var cakes = new[] { cake, cake };

var r = Csv.GetText(cakes, ("Name", c => c.Name), ("Method", c => c.Recipe));

Console.WriteLine(r);

public class Cake
{
    public Cake(string name, string recipe){
        this.Name = name;
        this.Recipe = recipe;
    }

    public string Name { get; init; }
    public string Recipe { get; init; }

    public override string ToString()
    {
        return $"To make a {Name} cake you must: {Recipe}";
    }
}