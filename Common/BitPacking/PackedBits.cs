/// <summary>Base class for bit packing.</summary>
public abstract class PackedBits
{
    /// <summary>Number of bits used by this class.</summary>
    protected readonly int NumberOfBits;

    /// <summary>Number of occupied bits.</summary>
    public int BitsOccupied { get; protected set; }
    /// <summary>Current bit pointer.</summary>
    public int BitPointer { get; protected set; }
    /// <summary>Number of bits still available for packing.</summary>
    public int BitsLeft => NumberOfBits - BitsOccupied;

    /// <summary>Creates empty instance for packing.</summary>
    public PackedBits(int numberOfBits)
    {
        this.NumberOfBits = numberOfBits;
        ResetPointer(isPacked: false);
    }

    /// <summary>Resets both packed value and bit pointer.</summary>
    public virtual void Reset()
    {
        ResetPointer(false);
        BitsOccupied = 0;
    }
    /// <summary>Resets bit pointer.</summary>
    /// <param name="isPacked">Is this instance already packed.</param>
    public void ResetPointer(bool isPacked)
    {
        BitPointer = isPacked ? BitsOccupied : 0;
    }
}
