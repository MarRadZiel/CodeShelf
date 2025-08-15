/// <summary>Multiple values packed in 32 bits.</summary>
public class PackedBits32 : PackedBits
{
    private const int bits32 = 32;
    private const int bits16 = 16;
    private const int bits8 = 8;
    private const int bit = 1;

    private int packedValue;

    /// <summary>Creates empty instance for packing.</summary>
    public PackedBits32() : base(bits32) { }
    /// <summary>Creates packed instance for unpacking.</summary>
    /// <param name="packedValue">Packed bits value.</param>
    public PackedBits32(int packedValue, int occupiedBits = bits32) : this()
    {
        if (occupiedBits < 0 || occupiedBits > bits32)
        {
            throw new ArgumentException($"Parameter {nameof(occupiedBits)} value passed to {nameof(PackedBits32)} constructor is out of range.");
        }
        this.packedValue = packedValue;
        BitsOccupied = occupiedBits;
        ResetPointer(isPacked: true);
    }

    /// <summary>Packs bool (as 1b) value.</summary>
    /// <param name="value">Value to be packed.</param>
    /// <returns>This instance for operation chaining.</returns>
    /// <exception cref="System.InvalidOperationException">Thrown when there is not enough empty bits to pack value.</exception>
    public PackedBits32 Pack(bool value)
    {
        return PackValue(value ? 1 : 0, bit);
    }
    /// <summary>Packs byte (8b) value.</summary>
    /// <param name="value">Value to be packed.</param>
    /// <returns>This instance for operation chaining.</returns>
    /// <exception cref="System.InvalidOperationException">Thrown when there is not enough empty bits to pack value.</exception>
    public PackedBits32 Pack(byte value)
    {
        return PackValue(value, bits8);
    }
    /// <summary>Packs short (16b) value.</summary>
    /// <param name="value">Value to be packed.</param>
    /// <returns>This instance for operation chaining.</returns>
    /// <exception cref="System.InvalidOperationException">Thrown when there is not enough empty bits to pack value.</exception>
    public PackedBits32 Pack(short value)
    {
        return PackValue(value, bits16);
    }
    /// <summary>Packs ushort (16b) value.</summary>
    /// <param name="value">Value to be packed.</param>
    /// <returns>This instance for operation chaining.</returns>
    /// <exception cref="System.InvalidOperationException">Thrown when there is not enough empty bits to pack value.</exception>
    public PackedBits32 Pack(ushort value)
    {
        return PackValue(value, bits16);
    }
    private PackedBits32 PackValue(int value, int size)
    {
        if (BitsOccupied + size > NumberOfBits)
        {
            throw new InvalidOperationException($"Not enough empty bits to pack {size}-bit value.");
        }

        if (BitPointer > 0)
        {
            packedValue <<= size;
        }

        packedValue |= value & ((1 << size) - 1);
        BitPointer += size;
        BitsOccupied += size;
        return this;
    }

    /// <summary>Unpacks byte (8b) value.</summary>
    /// <param name="value">Unpacked value.</param>
    /// <returns>This instance for operation chaining.</returns>
    /// <exception cref="System.InvalidOperationException">Thrown when there is not enough packed bits to unpack value.</exception>
    public PackedBits32 UnpackBool(out bool value)
    {
        value = UnpackValue(bit) != 0;
        return this;
    }
    /// <summary>Unpacks byte (8b) value.</summary>
    /// <returns>Unpacked value.</returns>
    /// <exception cref="System.InvalidOperationException">Thrown when there is not enough packed bits to unpack value.</exception>
    public bool UnpackBool()
    {
        return UnpackValue(bit) != 0;
    }
    /// <summary>Unpacks byte (8b) value.</summary>
    /// <param name="value">Unpacked value.</param>
    /// <returns>This instance for operation chaining.</returns>
    /// <exception cref="System.InvalidOperationException">Thrown when there is not enough packed bits to unpack value.</exception>
    public PackedBits32 UnpackByte(out byte value)
    {
        value = (byte)UnpackValue(bits8);
        return this;
    }
    /// <summary>Unpacks byte (8b) value.</summary>
    /// <returns>Unpacked value.</returns>
    /// <exception cref="System.InvalidOperationException">Thrown when there is not enough packed bits to unpack value.</exception>
    public byte UnpackByte()
    {
        return (byte)UnpackValue(bits8);
    }
    /// <summary>Unpacks short (16b) value.</summary>
    /// <param name="value">Unpacked value.</param>
    /// <returns>This instance for operation chaining.</returns>
    /// <exception cref="System.InvalidOperationException">Thrown when there is not enough packed bits to unpack value.</exception>
    public PackedBits32 UnpackShort(out short value)
    {
        value = (short)UnpackValue(bits16);
        return this;
    }
    /// <summary>Unpacks short (16b) value.</summary>
    /// <returns>Unpacked value.</returns>
    /// <exception cref="System.InvalidOperationException">Thrown when there is not enough packed bits to unpack value.</exception>
    public short UnpackShort()
    {
        return (short)UnpackValue(bits16);
    }
    /// <summary>Unpacks ushort (16b) value.</summary>
    /// <param name="value">Unpacked value.</param>
    /// <returns>This instance for operation chaining.</returns>
    /// <exception cref="System.InvalidOperationException">Thrown when there is not enough packed bits to unpack value.</exception>
    public PackedBits32 UnpackUShort(out ushort value)
    {
        value = (ushort)UnpackValue(bits16); ;
        return this;
    }
    /// <summary>Unpacks ushort (16b) value.</summary>
    /// <returns>Unpacked value.</returns>
    /// <exception cref="System.InvalidOperationException">Thrown when there is not enough packed bits to unpack value.</exception>
    public ushort UnpackUShort()
    {
        return (ushort)UnpackValue(bits16);
    }
    private int UnpackValue(int size)
    {
        if (BitPointer < size)
        {
            throw new InvalidOperationException($"Not enough packed bits to unpack {size}-bit value.");
        }

        int mask = (1 << size) - 1;
        int value = (packedValue >> (BitPointer - size)) & mask;
        BitPointer -= size;
        return value;
    }

    /// <inheritdoc/>
    public override void Reset()
    {
        base.Reset();
        packedValue = 0;
    }

    public override string ToString()
    {
        return packedValue.ToString();
    }
    public string ToString(bool asBits)
    {
        if (asBits)
        {
            return System.Convert.ToString(packedValue, 2).PadLeft(NumberOfBits, '0');
        }
        return packedValue.ToString();
    }

    public static implicit operator int(PackedBits32 packedBits32)
    {
        return packedBits32.packedValue;
    }
}
