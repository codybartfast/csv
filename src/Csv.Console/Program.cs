using System.Text;
using Fmbm.Csv;

// See https://aka.ms/new-console-template for more information
Console.WriteLine("Hello, World!");

var txt = File.ReadAllText("fruit.csv");
var reader = new CharReader(txt);
var sb = new StringBuilder();
while(reader.TryRead(out var c)){
    sb.Append(c);
}
Console.WriteLine(sb);
Console.WriteLine(int.Parse(" 33 "));