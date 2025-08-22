
ByteSize byteSizeA = 5 * ByteSize.MB2B + 23 * ByteSize.KB2B + 123; // implicit cast from long to ByteSize
ByteSize byteSizeB = new ByteSize(123456789);

Console.WriteLine($"A: {byteSizeA}");
Console.WriteLine($"B: {byteSizeB}");
Console.WriteLine("-Formatting-");
Console.WriteLine($"A(F3): {byteSizeA:F3}");
Console.WriteLine($"A(KB2): {byteSizeA:KB2}");
Console.WriteLine("-Operations-");
Console.WriteLine($"A-B = {(byteSizeA - byteSizeB).ToString(allowNegative: true)}");
Console.WriteLine($"A.SubtractSafe(B) = {byteSizeA.SubtractSafe(byteSizeB).ToString(allowNegative: true)}");
Console.WriteLine($"B-A = {(byteSizeB - byteSizeA).ToString(allowNegative: true)}");
Console.WriteLine($"B.SubtractSafe(A) = {byteSizeB.SubtractSafe(byteSizeA).ToString(allowNegative: true)}");
Console.WriteLine($"A+B = {byteSizeA + byteSizeB}");
Console.WriteLine("-Min and Max values-");
Console.WriteLine($"Min: {ByteSize.MinValue} ({ByteSize.MinValue.TotalBytes})");
Console.WriteLine($"Min: {ByteSize.MinValue.ToString(allowNegative: true)} ({ByteSize.MinValue.TotalBytes})");
Console.WriteLine($"Max: {ByteSize.MaxValue} ({ByteSize.MaxValue.TotalBytes})");
Console.WriteLine("-Parsing-");
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

ByteSize byteSizeC = 16 * ByteSize.MB2B + 32 * ByteSize.KB2B + 64;
Console.WriteLine($"A(F3): {byteSizeC:F3}");
Console.WriteLine($"A(KB2): {byteSizeC:KB2}");


Console.ReadKey();
