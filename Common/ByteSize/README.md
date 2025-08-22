# Byte Size
[ByteSize](ByteSize.cs) is a lightweight and expressive struct for representing and manipulating byte quantities in .NET. It supports intuitive arithmetic, formatting, parsing, and boundary-safe operations — all with human-readable output.

## 🚀 Quick Start
This section demonstrates how to create and display `ByteSize` instances.  
The first one is created by implicitly casting a `long` value to `ByteSize`. The value is treated as a total number of bytes. Constants `MB2B` and `KB2B` are used to convert megabytes and kilobytes to bytes. 
These are predefined constants in `ByteSize` representing the number of bytes in a megabyte/kilobyte.  
The second one is created using the constructor that accepts total number of bytes.

```csharp
ByteSize byteSizeA = 5 * ByteSize.MB2B + 23 * ByteSize.KB2B + 123; // implicit cast from long to ByteSize
ByteSize byteSizeB = new ByteSize(123456789);

Console.WriteLine($"A: {byteSizeA}");
Console.WriteLine($"B: {byteSizeB}");
```
### Output
<pre>
A: 5MB 23KB 123B
B: 117MB 755KB 277B
</pre>

## 🎨 Formatting options
`ByteSize` supports multiple formatting styles via `ToString(string format)`:
- `G` - grouped units, e.g. `"16MB 32KB 64B"`.
- `B` - total bytes, e.g. `"16810048B"`.
- `F` - floating-point using largest applicable unit, e.g. `"16,03MB"`; supports precision via `F0`-`F9`.
- `KB`, `MB`, `GB`, `TB`, `PB` - force specific unit, e.g. `"16416,06KB"`; supports precision via `KB0`-`KB9`.

```csharp
Console.WriteLine($"A(F3): {byteSizeA:F3}");
Console.WriteLine($"A(KB2): {byteSizeA:KB2}");
```
### Output
<pre>
A(F3): 5.023MB
A(KB2): 5143.12KB
</pre>

## ➕➖ Arithmetic operations
This section demonstrates how to perform arithmetic with `ByteSize` instances.
- The `-` operator subtracts the total bytes of one instance from another and **can produce negative results**.  
By default, `ToString()` will display `< 0B` for negative values. Using `allowNegative: true` shows the full breakdown into byte units, with a minus sign.
- The `SubtractSafe()` method performs the same subtraction but **clamps the result to zero** if it would otherwise be negative.
- The `+` operator adds the total bytes of two instances.

```csharp
Console.WriteLine($"A-B = {(byteSizeA - byteSizeB).ToString(allowNegative: true)}");
Console.WriteLine($"A.SubtractSafe(B) = {byteSizeA.SubtractSafe(byteSizeB).ToString(allowNegative: true)}");
Console.WriteLine($"B-A = {(byteSizeB - byteSizeA).ToString(allowNegative: true)}");
Console.WriteLine($"B.SubtractSafe(A) = {byteSizeB.SubtractSafe(byteSizeA).ToString(allowNegative: true)}");
Console.WriteLine($"A+B = {byteSizeA + byteSizeB}");
```
### Output
<pre>
A-B = - 112MB 732KB 154B
A.SubtractSafe(B) = 0B
B-A = 112MB 732KB 154B
B.SubtractSafe(A) = 112MB 732KB 154B
A+B = 122MB 778KB 400B
</pre>

## 📏 Minimum and Maximum `ByteSize` Values
This snippet demonstrates how to retrieve and display the smallest and largest values that a `ByteSize` instance can represent.
- `ByteSize.MinValue` – The lowest possible value, based on the underlying `long` storage.  
By default, negative values are displayed as `< 0B` unless `allowNegative: true` is specified.  
- `ByteSize.MaxValue` – The highest possible value, also based on the maximum `long` value, displayed in full unit breakdown.

```csharp
Console.WriteLine($"Min: {ByteSize.MinValue.ToString(allowNegative: true)} ({ByteSize.MinValue.TotalBytes})");
Console.WriteLine($"Max: {ByteSize.MaxValue} ({ByteSize.MaxValue.TotalBytes})");
```
### Output
<pre>
Min: - 8191PB 1023TB 1023GB 1023MB 1023KB 1023B  (-9223372036854775807)
Max: 8191PB 1023TB 1023GB 1023MB 1023KB 1023B  (9223372036854775807)
</pre>

## 🔍 Parsing a `ByteSize` from a `String`
This snippet shows how to convert a human-readable size string into a `ByteSize` instance using `ByteSize.TryParse`.

`ByteSize.TryParse(string, out result)` attempts to parse the given string into a `ByteSize`.  
Returns `true` if parsing succeeds, with the parsed value in `result`.  
Returns `false` if the string is invalid, allowing you to handle the error without throwing an exception.  
**Note:** Bit units (e.g., `Mb`) are unsupported. All other casing variants of byte units (`MB`,`mB` and `mb`) are treated as bytes.

```csharp
string stringToParse = "- 1.123GB 25MB";
Console.WriteLine($"Parsing: \"{stringToParse}\"");

if (ByteSize.TryParse(stringToParse, out var result))
{
    Console.WriteLine(result!.Value.ToString(true));
}
else
{
    Console.WriteLine("Parsing error");
}
```
### Output
<pre>
Parsing: "- 1.123GB 25MB"
- 1GB 150MB 974KB 868B
</pre>


## Sample
Full sample code is available in [Sample.cs](Sample.cs)
