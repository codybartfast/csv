module FSTests

open System.Globalization
open Fmbm.Text
open Xunit

let usClt = CultureInfo.GetCultureInfo("en-US");
let frClt = CultureInfo.GetCultureInfo("fr-FR");

let csvText =
    @"Id,Name,Price
1,Apple,""1,100""
2,Banana,""2,200""
3,Cherry,""3,300""
4,Durian,""4,400"""

[<Fact>]
let ``Basics`` () =
    let items =
        Csv.getItems (
            csvText,
            usClt,
            fun row ->
                {| Id = "Id" |> row |> int
                   Name = "Name" |> row |> string
                   Price = "Price" |> row |> decimal |}
        ) |> Array.ofSeq

    Assert.Equal(4, items.Length)
    Assert.Equal("Banana", items.[1].Name)

    let text2 =
        Csv.getText(
            items |> Array.take 2,
            usClt,
            [
                ("Id", fun f -> f.Id)
                ("Name", fun f -> f.Name)
                ("Price", fun f -> f.Price) ]
        )
    Assert.Equal("Id,Name,Price\n1,Apple,1100\n2,Banana,2200\n", text2)
