Console.WriteLine($"- 32-bit packing -");
{
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
}
Console.WriteLine();

Console.WriteLine($"- 64-bit packing -");
{
    byte a = 56;
    short b = 26598;
    byte c = 165;
    int d = 3095092;
    Console.WriteLine($"Original values: a={a} b={b} c={c} d={d}");
    PackedBits64 packed64 = new PackedBits64();
    Console.WriteLine($" {packed64.ToString(asBits: true)} ({packed64})\t BitPointer:{packed64.BitPointer} BitsLeft:{packed64.BitsLeft}");
    packed64.Pack(a).Pack(b).Pack(c).Pack(d);
    Console.WriteLine($" {packed64.ToString(asBits: true)} ({packed64})\t BitPointer:{packed64.BitPointer} BitsLeft:{packed64.BitsLeft}");
    packed64.UnpackByte(out var a2).UnpackShort(out var b2).UnpackByte(out var c2).UnpackInt(out var d2);
    Console.WriteLine($"Unpacked values: a={a2} b={b2} c={c2} d={d2}");
    Console.WriteLine($" {packed64.ToString(asBits: true)} ({packed64})\t BitPointer:{packed64.BitPointer} BitsLeft:{packed64.BitsLeft}");
    try
    {
        packed64.Pack(true); // exceeding packing limit by 1 bit
    }
    catch (InvalidOperationException ex)
    {
        Console.WriteLine($"Expected error: {ex.Message}");
    }
    packed64.Reset();
    Console.WriteLine($" {packed64.ToString(asBits: true)} ({packed64})\t BitPointer:{packed64.BitPointer} BitsLeft:{packed64.BitsLeft}");
}

Console.ReadKey();