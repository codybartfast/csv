using Fmbm.Csv;

Console.WriteLine("Hello, World!");



// var cake1 = new Cake("Fruit", "Bake", DateTime.Parse("2022-06-23T15:23"), 6);
// var cake2 = new Cake("Victoria", "Stir", DateTime.Now, 400000000);
// var cakes = new[] { cake1, cake2 };

// var r = Csv.GetText(cakes,
//     ("Name", c => c.Name),
//     ("Method", c => c.Recipe),
//     ("Added", c => c.Added),
//     ("Serves", c => c.Serves));

// Console.WriteLine(r);

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