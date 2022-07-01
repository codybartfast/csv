CSV
===

Basic reading and writing of CSV (comma-separated value) text to help share data
with other applications like spreadsheets applications.

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
The Bad Fish Paradigm,18, 1 ,"September 22, 2008",3T7351,9360364
The Barbarian Sublimation,20, 3 ,"October 6, 2008",3T7353,9329673
The Codpiece Topology,19, 2 ,"September 29, 2008",3T7352,8758200
The Cooper-Nowitzki Theorem,23, 6 ,"November 3, 2008",3T7356,9670118
The Euclid Alternative,22, 5 ,"October 20, 2008",3T7355,9280649
The Griffin Equivalency,21, 4 ,"October 13, 2008",3T7354,9356497
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

The Episode class used in the above:

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

The 'standard' types are:

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

`CultureInfo.CurrentCulture` is used by default to convert to and from text.

The culture can be specified by passing it as the second argument to either of
the above methods.  E.g. to explictly use `InvariantCulture`:

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
    ...);
```

&nbsp;

Automatic Conversion
--------------------

### Conversion To Text

`DateTime`: converts to text using the format 'yyyy-MM-dd HH:mm'. (This format
allows for time data to be included and is readable by most spreadsheet apps).

The standard numeric types are converted by calling `.ToString(<culture>)` on
the number.

Other types are converted by calling `.ToString()` on the object.

`null` is converted to an empty string.

### Conversion From Text

For the standard types conversion from text is done by implicit conversion
operators.

`DateTime`: first a TryParseExact is attempted using the default format
'yyyy-MM-dd HH:mm', if that fails then `Parse` is called using the provided or
default culture.  Leading and trailing whitespace is allowed.

Standard numeric types are converted by calling `Parse` on the numeric type with
the provided or default culture.  Leading and trailing whitespace, and thousands
separators are allowed.

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

string csvTextOut = Csv.GetText(episodes, CultureInfo.InvariantCulture,
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

var episodes = Csv.GetItems(csvTextIn, CultureInfo.InvariantCulture, row =>
    new Episode
    {
        ...
        OriginalAirDate = TextToDate(row("Original air date")),
        ...
    }).ToArray();
```

The `row` function returns a `Cell`. To explicitly get a string use its
`Text` property.  This example will fail because the implicit conversion to text
is not used:

```csharp
DateTime ObjectToDate(object dateObj)
{
    switch (dateObj)
    {
        case string text:
            return DateTime.ParseExact(text, "HH:mm on dd-MM-yyyy", null);
        default:
            throw new Exception(
                $"I don't know what to do with a '{dateObj.GetType()}'");
    }
}

var episodes = Csv.GetItems(csvTextIn, CultureInfo.InvariantCulture, row =>
    new Episode
    {
        ...
        OriginalAirDate = ObjectToDate(row("Original air date")),
        ...
    }).ToArray();
```

An exception is thrown:

```Text
Unhandled exception. System.Exception: I don't know what to do with a 'Fmbm.Text.Cell'
   at Program.<<Main>$>g__ObjectToDate|0_0(Object dateObj) in ...
```

But the following works as expected:

```csharp
var episodes = Csv.GetItems(csvTextIn, CultureInfo.InvariantCulture, row =>
    new Episode
    {
        ...
        OriginalAirDate = ObjectToDate(row("Original air date").Text),
        ...
    }).ToArray();
```

&nbsp;

Anonymous Types
---------------

`GetItems` can be used to generate anonymous types.  Use explicit casts
to determine the type of the properties:

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

Tables And Rows
---------------

Internally a `Table` is created whether converting from items to text
(`IEnumerable<TItem> -> Table -> string`) or from text to items
(`string -> Table -> IEnumerable<TItem>`).  `Fmbm.CSV` is not intend for editing
CSV tables but `Table` does enable some basic editing.  For example, to put
double quotes around the tites in the orignal CSV text:

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
arount `Title`.

Unlike reading and writing items, the format of the CSV is unchanged.  The
production code is still present, the date is in the original format and spaces
remain around the episode number.

### Table Tolerance

`GetTable` is more permisive than `GetItems` and can be used to read CSV text
that cannot be used by `GetItems`.  `GetItems` requires there is at
lease one row, that the items in the first row (the headers) are unique, and 
that all the rows are of the same length.  For example, this CSV can be read by
`GetTable`, but would cause `GetItems` to throw an exception.

```csv
Fruit,Fruit,Fruit
Apple,Banana,Cherry
Green,Yellow
,,,,
```

[Fubu]: <https://fubumvc.github.io/>
[BbtS2]: <https://en.wikipedia.org/wiki/List_of_The_Big_Bang_Theory_episodes#Season_2_(2008%E2%80%9309)>
