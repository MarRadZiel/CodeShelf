/// <summary>Multiple values packed in 64 bits.</summary>
public class PackedBits64 : PackedBits
{
    private const int bits64 = 64;
    private const int bits32 = 32;
    private const int bits16 = 16;
    private const int bits8 = 8;
    private const int bit = 1;

    private long packedValue;

    /// <summary>Creates empty instance for packing.</summary>
    public PackedBits64() : base(bits64) { }
    /// <summary>Creates packed instance for unpacking.</summary>
    /// <param name="packedValue">Packed bits value.</param>
    public PackedBits64(long packedValue, int occupiedBits = bits64) : this()
    {
        if (occupiedBits < 0 || occupiedBits > NumberOfBits)
        {
            throw new ArgumentException($"Parameter {nameof(occupiedBits)} value passed to {nameof(PackedBits64)} constructor is out of range.");
        }
        this.packedValue = packedValue;
        BitsOccupied = occupiedBits;
        ResetPointer(isPacked: true);
    }

    /// <summary>Packs bool (as 1b) value.</summary>
    /// <param name="value">Value to be packed.</param>
    /// <returns>This instance for operation chaining.</returns>
    /// <exception cref="System.InvalidOperationException">Thrown when there is not enough empty bits to pack value.</exception>
    public PackedBits64 Pack(bool value)
    {
        return PackValue(value ? 1 : 0, bit);
    }
    /// <summary>Packs byte (8b) value.</summary>
    /// <param name="value">Value to be packed.</param>
    /// <returns>This instance for operation chaining.</returns>
    /// <exception cref="System.InvalidOperationException">Thrown when there is not enough empty bits to pack value.</exception>
    public PackedBits64 Pack(byte value)
    {
        return PackValue(value, bits8);
    }
    /// <summary>Packs short (16b) value.</summary>
    /// <param name="value">Value to be packed.</param>
    /// <returns>This instance for operation chaining.</returns>
    /// <exception cref="System.InvalidOperationException">Thrown when there is not enough empty bits to pack value.</exception>
    public PackedBits64 Pack(short value)
    {
        return PackValue(value, bits16);
    }
    /// <summary>Packs ushort (16b) value.</summary>
    /// <param name="value">Value to be packed.</param>
    /// <returns>This instance for operation chaining.</returns>
    /// <exception cref="System.InvalidOperationException">Thrown when there is not enough empty bits to pack value.</exception>
    public PackedBits64 Pack(ushort value)
    {
        return PackValue(value, bits16);
    }
    /// <summary>Packs int (32b) value.</summary>
    /// <param name="value">Value to be packed.</param>
    /// <returns>This instance for operation chaining.</returns>
    /// <exception cref="System.InvalidOperationException">Thrown when there is not enough empty bits to pack value.</exception>
    public PackedBits64 Pack(int value)
    {
        return PackValue(value, bits32);
    }
    /// <summary>Packs uint (32b) value.</summary>
    /// <param name="value">Value to be packed.</param>
    /// <returns>This instance for operation chaining.</returns>
    /// <exception cref="System.InvalidOperationException">Thrown when there is not enough empty bits to pack value.</exception>
    public PackedBits64 Pack(uint value)
    {
        return PackValue(value, bits32);
    }
    private PackedBits64 PackValue(long value, int size)
    {
        if (BitsOccupied + size > NumberOfBits)
        {
            throw new InvalidOperationException($"Not enough empty bits to pack {size}-bit value.");
        }

        if (BitPointer > 0)
        {
            packedValue <<= size;
        }

        packedValue |= value & ((1L << size) - 1);
        BitPointer += size;
        BitsOccupied += size;
        return this;
    }

    /// <summary>Unpacks byte (8b) value.</summary>
    /// <param name="value">Unpacked value.</param>
    /// <returns>This instance for operation chaining.</returns>
    /// <exception cref="System.InvalidOperationException">Thrown when there is not enough packed bits to unpack value.</exception>
    public PackedBits64 UnpackBool(out bool value)
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
    public PackedBits64 UnpackByte(out byte value)
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
    public PackedBits64 UnpackShort(out short value)
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
    public PackedBits64 UnpackUShort(out ushort value)
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
    /// <summary>Unpacks int (32b) value.</summary>
    /// <param name="value">Unpacked value.</param>
    /// <returns>This instance for operation chaining.</returns>
    /// <exception cref="System.InvalidOperationException">Thrown when there is not enough packed bits to unpack value.</exception>
    public PackedBits64 UnpackInt(out int value)
    {
        value = (int)UnpackValue(bits32);
        return this;
    }
    /// <summary>Unpacks int (32b) value.</summary>
    /// <returns>Unpacked value.</returns>
    /// <exception cref="System.InvalidOperationException">Thrown when there is not enough packed bits to unpack value.</exception>
    public int UnpackInt()
    {
        return (int)UnpackValue(bits32);
    }
    /// <summary>Unpacks uint (32b) value.</summary>
    /// <param name="value">Unpacked value.</param>
    /// <returns>This instance for operation chaining.</returns>
    /// <exception cref="System.InvalidOperationException">Thrown when there is not enough packed bits to unpack value.</exception>
    public PackedBits64 UnpackUShort(out uint value)
    {
        value = (uint)UnpackValue(bits32); ;
        return this;
    }
    /// <summary>Unpacks uint (32b) value.</summary>
    /// <returns>Unpacked value.</returns>
    /// <exception cref="System.InvalidOperationException">Thrown when there is not enough packed bits to unpack value.</exception>
    public uint UnpackUInt()
    {
        return (uint)UnpackValue(bits32);
    }
    private long UnpackValue(int size)
    {
        if (BitPointer < size)
        {
            throw new InvalidOperationException($"Not enough packed bits to unpack {size}-bit value.");
        }

        long mask = (1L << size) - 1;
        long value = (packedValue >> (BitPointer - size)) & mask;
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

    public static implicit operator long(PackedBits64 packedBits64)
    {
        return packedBits64.packedValue;
    }
}
