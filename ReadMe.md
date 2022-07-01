CSV
===

Basic reading and writing of CSV (comma-separated value) text to help share data
with other applications like Excel.

Features:

* Create objects from CSV text and create CSV text from objects.
* Implicit conversion for standard types: string, DateTime, int, uint, long,
  ulong, float, double and decimal.
* Other types are supported with explicit conversion.
* User specified cutlure. (Defaults to `CurrentCultue`.)
* Intermediate data structures: `Table`, `Row`, and `Cell`, are available for
  rudimentary editing of CSV data.

&nbsp;

For Me, By Me (FMBM)
--------------------

FMBM packages are created primarily for use by the author.  They are intended
for getting casual, desktop applications up and running quickly.  They may not
be suitable for production, scalable nor critical applications. The name is
inspired by the [Fubu][Fubu], _For Us, By Us_, project, but there is no other
connection.

&nbsp;

Contents
--------

[Basic Usage](#basic-usage)  

&nbsp;

Basic Usage
-----------

### Creating Objects from CSV Text

Given this CSV text for the [second season][BbtS2] of The Big Bang Theory:

```csv
Title,No. overall,No. in season,Original air date,Prod. code,U.S. viewers
The Bad Fish Paradigm,18,1,"September 22, 2008",3T7351,9360364
The Barbarian Sublimation,20,3,"October 6, 2008",3T7353,9329673
The Codpiece Topology,19,2,"September 29, 2008",3T7352,8758200
The Cooper-Nowitzki Theorem,23,6,"November 3, 2008",3T7356,9670118
The Euclid Alternative,22,5,"October 20, 2008",3T7355,9280649
The Griffin Equivalency,21,4,"October 13, 2008",3T7354,9356497
```

We can create `Episode` objects from this CSV Text using `CSV.GetItems`:

```csharp
Episode[] episodes = Csv.GetItems(csvTextIn, row =>
    new Episode
    {
        NumOverall = row("No. overall"),
        NumInSeason = row("No. in season"),
        Title = row("Title"),
        OriginalAirDate = row("Original air date"),
        USViewers = row("U.S. viewers")
    }).ToArray();
```

`GetItems<TItem>` takes the CSV text and an `itemMaker` function.  `itemMaker`
is a function which is given a `row` function which looks up the value of a
given field in that row, the `itemMaker`uses those values to construct an item.
`row` is a `Func<string, Cell>` (string -> Cell), and `itemMaker` is a
`Func<Func<string, Cell>, Item>` ((string -> Cell) -> Item).

&nbsp;

### Creating CSV Text from Objects

We can create CSV text from these objects using `Csv.GetText`:

```csharp
string csvTextOut = Csv.GetText(episodes,
    ("No. Overall", ep => ep.NumOverall),
    ("No. In Season", ep => ep.NumInSeason),
    ("Title", ep => ep.Title),
    ("Original Air Date", ep => ep.OriginalAirDate),
    ("US Viewers", ep => ep.USViewers));
```

This creates the following CSV text:

```csv
No. Overall,No. In Season,Title,Original Air Date,US Viewers
18,1,The Bad Fish Paradigm,2008-09-22 00:00,9360364
20,3,The Barbarian Sublimation,2008-10-06 00:00,9329673
19,2,The Codpiece Topology,2008-09-29 00:00,8758200
23,6,The Cooper-Nowitzki Theorem,2008-11-03 00:00,9670118
22,5,The Euclid Alternative,2008-10-20 00:00,9280649
21,4,The Griffin Equivalency,2008-10-13 00:00,9356497
```

`Csv.GetText<TItem>` takes the `items` to be translated and an arbitrary number
of tuples that describe each column of the CSV text.  The tuple comprises the
header for the column and a function that gets the value for that column for a
given item. The full type is `(string header, Func<TItem, object> getValue)`.
In the example above.  The first column info is `("No. Overall", ep =>
ep.NumOverall)`.  That is, the header of the first column is `"No. Overall"` and
its value is obtained form the `NumOverall` property.

CLASS

CELL



Not for editing or two way mod
Custom converters, percent, culture!
scientific
null & "" -> ""
culture 
anonymouse types

[Fubu]: <https://fubumvc.github.io/>
[BbtS2]: <https://en.wikipedia.org/wiki/List_of_The_Big_Bang_Theory_episodes#Season_2_(2008%E2%80%9309)>
