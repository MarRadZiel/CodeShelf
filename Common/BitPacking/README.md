# Bit packing
[PackedBits32](PackedBits32.cs) and [PackedBits64](PackedBits64.cs) classes allow packing multiple values into a single one (a 32-bit integer or a 64-bit long integer, respectively).<br/>
They can be used to generate compact, unique identifiers by encoding multiple smaller values into a single integer. 

## Sample
This sample demonstrates how to use [PackedBits32](PackedBits32.cs) class to pack and unpack multiple values into a single 32-bit integer.<br/>
Full sample code is available in [Program.cs](Program.cs)

### Sample code
This code fragment shows how packing and unpacking affects the value of [PackedBits32](PackedBits32.cs) instance.<br/>
BitPointer indicates the current position in the bit stream, while BitsLeft shows how many bits remain available for packing.

```csharp
byte a = 255;
short b = 1;
bool c = false;
bool d = true;
Console.WriteLine($"Original values: a={a} b={b} c={c} d={d}");
PackedBits32 packed32 = new PackedBits32();
Console.WriteLine($" {packed32.ToString(asBits: true)} ({packed32})\t BitPointer:{packed32.BitPointer} BitsLeft:{packed32.BitsLeft}");
// Pack values
Console.WriteLine($"Packed: {a}");
packed32.Pack(a);
Console.WriteLine($" {packed32.ToString(asBits: true)} ({packed32})\t BitPointer:{packed32.BitPointer} BitsLeft:{packed32.BitsLeft}");
Console.WriteLine($"Packed: {b}");
packed32.Pack(b);
Console.WriteLine($" {packed32.ToString(asBits: true)} ({packed32})\t BitPointer:{packed32.BitPointer} BitsLeft:{packed32.BitsLeft}");
Console.WriteLine($"Packed: {c}");
packed32.Pack(c);
Console.WriteLine($" {packed32.ToString(asBits: true)} ({packed32})\t BitPointer:{packed32.BitPointer} BitsLeft:{packed32.BitsLeft}");
Console.WriteLine($"Packed: {d}");
packed32.Pack(d);
Console.WriteLine($" {packed32.ToString(asBits: true)} ({packed32})\t BitPointer:{packed32.BitPointer} BitsLeft:{packed32.BitsLeft}");
// Unpack values in the same order
packed32.UnpackByte(out var a2).UnpackShort(out var b2).UnpackBool(out var c2).UnpackBool(out var d2);
Console.WriteLine($"Unpacked values: a={a2} b={b2} c={c2} d={d2}");
Console.WriteLine($" {packed32.ToString(asBits: true)} ({packed32})\t BitPointer:{packed32.BitPointer} BitsLeft:{packed32.BitsLeft}");
Console.WriteLine($"Reset pointer to packed state (for re-unpacking).");
packed32.ResetPointer(isPacked: true);
Console.WriteLine($" {packed32.ToString(asBits: true)} ({packed32})\t BitPointer:{packed32.BitPointer} BitsLeft:{packed32.BitsLeft}");
// Reset packed value if needed
Console.WriteLine($"Reset");
packed32.Reset();
Console.WriteLine($" {packed32.ToString(asBits: true)} ({packed32})\t BitPointer:{packed32.BitPointer} BitsLeft:{packed32.BitsLeft}");
```

### Sample Output

<pre>Original values: a=255 b=1 c=False d=True
 00000000000000000000000000000000 (0)    BitPointer:0 BitsLeft:32
Packed: 255
 000000000000000000000000<strong><i>11111111</i></strong> (255)  BitPointer:8 BitsLeft:24
Packed: 1
 0000000011111111<strong><i>0000000000000001</i></strong> (16711681)     BitPointer:24 BitsLeft:8
Packed: False
 0000000111111110000000000000001<strong><i>0</i></strong> (33423362)     BitPointer:25 BitsLeft:7
Packed: True
 0000001111111100000000000000010<strong><i>1</i></strong> (66846725)     BitPointer:26 BitsLeft:6
Unpacked values: a=255 b=1 c=False d=True
 00000011111111000000000000000101 (66846725)     BitPointer:0 BitsLeft:6
Reset pointer to packed state (for re-unpacking).
 00000011111111000000000000000101 (66846725)     BitPointer:26 BitsLeft:6
Reset
 00000000000000000000000000000000 (0)    BitPointer:0 BitsLeft:32</pre>
