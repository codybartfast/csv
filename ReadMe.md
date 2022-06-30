CSV
===

Basic reading and writing of CSV (comma-separated value) text to help share data
with other applications like Excel.

Features:

* Simple conversion form objects to CSV text and from CSV text to objects
* Implicit conversion for standard types: string, DateTime, int, uint, long,
  ulong, float, double and decimal.
* Conversion can be customized for standard or other types.
* Generally you can ignore the internal data structures (Table, Row, Cell) but
  they are accessible.

Limitations:

* Fmbm.Csv is _not_ designed for editing data that will also be edited by a
  separate application like Excel.  It will only write data for the fields it
  knows about and so could lose data if additional columns were added by an
  applicaiton like Excel.
* It uses the InvarientCulture to read and

Csv seemed like a good idea at 2022-06-23T08:54:30.3216162Z.

Not for editing or two way mod
pass a function for di
Custom converters, percent, culture!
scientific
null & "" -> ""
From / FromAny
culture - invarient limited testing