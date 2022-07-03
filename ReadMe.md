CSV
===

Basic reading and writing of CSV (comma-separated value) text to help share data
with other applications like spreadsheets applications.

Features:

* Create objects from CSV text and create CSV text from objects.
* Implicit conversion for standard types (string, DateTime, int, uint, long,
  ulong, float, double and decimal).
* Other types are supported with explicit conversion.
* Specify the culture used for concersion. (Defaults to `CurrentCultue`.)
* Intermediate data structures: `Table`, `Row`, and `Cell`, are available for
  rudimentary editing of CSV text.

&nbsp;

For Me, By Me (FMBM)
--------------------

FMBM packages are created primarily for use by the author.  They are intended
for getting casual, desktop applications up and running quickly.  They may not
be suitable for production, scalable nor critical applications. The name is
inspired by the [Fubu][Fubu] project, '_For Us, By Us_', but there is no other
connection.

&nbsp;

Contents
--------

* [Basic Usage](#basic-usage)  
  * [Creating Objects from CSV Text](#creating-objects-from-csv-text)  
  * [Creating CSV Text from Objects](#creating-csv-text-from-objects)  
* [Standard Types](#standard-types)  
* [Culture](#culture)  
* [Automatic Conversion](#automatic-conversion)  
* [Custom Conversion](#custom-conversion)  
* [Anonymous Types](#anonymous-types)  
* [Tables and Rows](#tables-and-rows)  
* [Any to Any](#any-to-any)

&nbsp;

Basic Usage
-----------

### Creating Objects from CSV Text

Given this CSV text for some episodes from the [second season][BbtS2] of The Big
Bang Theory:

```csv
Title,No. overall,No. in season,Original air date,Prod. code,U.S. viewers
The Bad Fish Paradigm,18, 1 ,"September 22, 2008",3T7351,9360364
The Barbarian Sublimation,20, 3 ,"October 6, 2008",3T7353,9329673
The Codpiece Topology,19, 2 ,"September 29, 2008",3T7352,8758200
The Cooper-Nowitzki Theorem,23, 6 ,"November 3, 2008",3T7356,9670118
The Euclid Alternative,22, 5 ,"October 20, 2008",3T7355,9280649
The Griffin Equivalency,21, 4 ,"October 13, 2008",3T7354,9356497
```

We can create `Episode` objects from this CSV Text using `CSV.GetItems`:

```csharp
using Fmbm.Text;

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

Signature:

```csharp
// (string, ((string -> Cell) -> TItem)) -> IEnumerable<Item>
public static IEnumerable<TItem> GetItems<TItem>(
        string csvText,
        Func<Func<string, Cell>, TItem> itemMaker)
//          |------ row ------|
```

`itemMaker` is called once for each row of the CSV text (except for the header
row).  `itemMaker` is given a `row` function and returns a `TItem`.

`row` takes a header name and returns a `Cell` containing the text for that
position.

&nbsp;

### Creating CSV Text from Objects

CSV text can be created from objects using  `Csv.GetText`:

```csharp
using Fmbm.Text;

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

Signature:

```csharp
//  (IEnumerable<TItem>, (columnInfo, columnInfo, ...)) -> string
    public static string GetText<TItem>(
        IEnumerable<TItem> items,
        params (string, Func<TItem, object>)[] columnInfos)
```

Each `columnInfo` is a tuple comprising the header for a column and a function
that gets the value for that column from a `TItem`. In the example above.  The
first columnInfo is `("No. Overall", ep => ep.NumOverall)`.  That is, the
header of the first column is `"No. Overall"` and the value for that column
is obtained form the `NumOverall` property of each episode.

The Episode class used in the above examples is:

```csharp
class Episode
{
    public long NumOverall { get; set; }
    public int NumInSeason { get; set; }
    public string Title { get; set; }
    public DateTime OriginalAirDate { get; set; }
    public decimal USViewers { get; set; }
}
```

&nbsp;

Standard Types
--------------

For 'standard' type the `Cell` does the conversion between the text value it
holds and the required type.

The standard types are:

`string`  
`DateTime`  
`int`  
`uint`  
`long`  
`ulong`  
`float`  
`double`  
`decimal`  

&nbsp;

Culture
-------

For standard types `CultureInfo.CurrentCulture` is used by default to convert to
and from text.

The culture can be specified by passing it as the second argument to `GetItems`
of `GetText`.  E.g. to explictly use `InvariantCulture`:

```csharp
var episodes = Csv.GetItems(csvTextIn, CultureInfo.InvariantCulture, row =>
    new Episode
    {
        NumOverall = row("No. overall"),
        ...
    }).ToArray();
```

```csharp
string csvTextOut = Csv.GetText(episodes, CultureInfo.InvariantCulture,
    ("No. Overall", ep => ep.NumOverall),
    ...
    );
```

&nbsp;

Automatic Conversion
--------------------

### Conversion To Text

`DateTime`: converts to text using the format `yyyy-MM-dd HH:mm`. (This format
allows for time data to be included and is readable by most spreadsheet apps).

The standard numeric types are converted by calling `.ToString(<culture>)` on
the number.

Other types are converted by calling `.ToString()` on the object.

`null` is converted to an empty string.

### Conversion From Text

For the standard types conversion from text is done by implicit conversion
operators.

`DateTime`: first a TryParseExact is attempted using the default format
`yyyy-MM-dd HH:mm`, if that fails then `Parse` is called using the provided or
the default culture.  Leading and trailing whitespace is allowed.

Standard numeric types are converted by calling `Parse` on the numeric type with
the provided or the default culture.  Leading and trailing whitespace, and
thousands separators are allowed.

There is no automatic conversion from text to non-standard types.

&nbsp;

Custom Conversion
-----------------

Non-standard types, and custom text formats can be supported by explictly
providing the CVS text or by parsing from the CVS text.  For example, to store
the original air date in the form `"20:00 on 15-08-2008"`:

Converting to CSV text:

```csharp
String DateToText(DateTime date){
    return date.ToString("HH:mm on dd-MM-yyyy");
}

string csvTextOut = Csv.GetText(episodes,
    ...
    ("Original Air Date", ep => DateToText(ep.OriginalAirDate)),
    ...
    );
```

Converting back to a DateTime:

```csharp
DateTime TextToDate(string text)
{
    return DateTime.ParseExact(text, "HH:mm on dd-MM-yyyy", null);
}

var episodes = Csv.GetItems(csvTextIn, row =>
    new Episode
    {
        ...
        OriginalAirDate = TextToDate(row("Original air date")),
        ...
    }).ToArray();
```

Similar conversion methods can be used to convert other types to and from
CSV text.

The `row` method returns a `Cell`.  To explicitly access its content as a string
use the `Text` property.  For example:

```csharp
var episodes = Csv.GetItems(csvTextIn, CultureInfo.InvariantCulture, row =>
    new Episode
    {
        ...
        OriginalAirDate = TextToDate(row("Original air date").Text),
        ...
    }).ToArray();
```

Using the `Text` property is not needed in this case but would be necessary if
the conversion function took an `object`, in that case the cell's implicit
conversion would not be called.

&nbsp;

Anonymous Types
---------------

`GetItems` can be used to generate anonymous types.  Use explicit casts to
specify the type of the properties:

```csharp
var episodes = Csv.GetItems(csvTextIn, row =>
    new
    {
        NumOverall = (long)row("No. overall"),
        NumInSeason = (int)row("No. in season"),
        Title = (string)row("Title"),
        OriginalAirDate = (DateTime)row("Original air date"),
        USViewers = (decimal)row("U.S. viewers")
    });
```

&nbsp;

Tables And Rows
---------------

Internally a `Table` is created both converting from items to text
(`IEnumerable<TItem> -> Table -> string`) or from text to items
(`string -> Table -> IEnumerable<TItem>`).  `Fmbm.CSV` is not intended for
editing CSV tables but `Table` does enable some basic editing.  For example, to
put double quotes around the tites in the orignal CSV text:

```csharp
var table = Csv.GetTable(csvTextIn);
foreach(var row in table.Rows.Skip(1)){
    row[0] = new Cell($"\"{row[0]}\"");
}
var csvTextOut = Csv.GetText(table);
```

```csv
Title,No. overall,No. in season,Original air date,Prod. code,U.S. viewers
"""The Bad Fish Paradigm""",18, 1 ,"September 22, 2008",3T7351,9360364
"""The Barbarian Sublimation""",20, 3 ,"October 6, 2008",3T7353,9329673
"""The Codpiece Topology""",19, 2 ,"September 29, 2008",3T7352,8758200
"""The Cooper-Nowitzki Theorem""",23, 6 ,"November 3, 2008",3T7356,9670118
"""The Euclid Alternative""",22, 5 ,"October 20, 2008",3T7355,9280649
"""The Griffin Equivalency""",21, 4 ,"October 13, 2008",3T7354,9356497
```

Note, it was necessary to `Skip` the first row to prevent putting double quotes
arount `Title` in the headers row.

Unlike reading and writing items to text, the format of the CSV text is
unchanged.  The production code is still present, the date is in the original
format and the space remain around the episode number.

&nbsp;

### Table Tolerance

`GetTable` is more permisive than `GetItems` and can be used to read CSV text
that cannot be used by `GetItems`.  `GetItems` requires there is at least one
row, that the items in the first row (the headers) are unique, and that all the
rows are of the same length.  For example, this CSV can be read by `GetTable`,
but would cause `GetItems` to throw an exception.

```csv
Fruit,Fruit,Fruit
Apple,Banana,Cherry
Green,Yellow
,,,,
```

&nbsp;

### Any to Any

`GetItems` creates items from CSV text, or from a table.  
`GetTable` creates a table from from CSV 'text, or from items.  
`GetText` creates CSV text from a table, or from items.  

[Fubu]: <https://fubumvc.github.io/>
[BbtS2]: <https://en.wikipedia.org/wiki/List_of_The_Big_Bang_Theory_episodes#Season_2_(2008%E2%80%9309)>
