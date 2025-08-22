using System;

/// <summary>Represents a data size value in bytes, with support for conversion between units (KB, MB, GB, TB, PB), 
/// arithmetic operations, formatting, and parsing from string representations.</summary>
public struct ByteSize : IEquatable<ByteSize>, IFormattable
{
    /// <summary>Represents the largest possible value of <see cref="ByteSize"/> which is equal to 9223372036854775807B (<see cref="long.MaxValue"/>).</summary>
    public static readonly ByteSize MaxValue = new ByteSize(long.MaxValue);
    /// <summary>Represents the smallest possible value of <see cref="ByteSize"/> which is equal to -9223372036854775807B (<see cref="long.MinValue"/>+1).</summary>
    public static readonly ByteSize MinValue = new ByteSize(long.MinValue + 1);

    public const long KB2B = 1024;
    public const long MB2B = 1048576;
    public const long GB2B = 1073741824;
    public const long TB2B = 1099511627776;
    public const long PB2B = 1125899906842624;

    public const double B2KB = 1.0 / 1024.0;
    public const double B2MB = 1.0 / 1048576.0;
    public const double B2GB = 1.0 / 1073741824.0;
    public const double B2TB = 1.0 / 1099511627776.0;
    public const double B2PB = 1.0 / 1125899906842624.0;

    private readonly long _totalBytes;
    public readonly long TotalBytes => _totalBytes;
    public readonly bool IsNegative => _totalBytes < 0;

    public int Bytes { get; private set; }
    public int KiloBytes { get; private set; }
    public int MegaBytes { get; private set; }
    public int GigaBytes { get; private set; }
    public int TeraBytes { get; private set; }
    public int PetaBytes { get; private set; }

    /// <summary>Gets the total size in kilobytes (KB)</summary>
    public readonly double TotalKiloBytes => _totalBytes * B2KB;
    /// <summary>Gets the total size in megabytes (MB)</summary>
    public readonly double TotalMegaBytes => _totalBytes * B2MB;
    /// <summary>Gets the total size in gigabytes (GB)</summary>
    public readonly double TotalGigaBytes => _totalBytes * B2GB;
    /// <summary>Gets the total size in terabytes (TB)</summary>
    public readonly double TotalTeraBytes => _totalBytes * B2TB;
    /// <summary>Gets the total size in petabytes (PB)</summary>
    public readonly double TotalPetaBytes => _totalBytes * B2PB;

    /// <summary>Creates a new instance of ByteSize.</summary>
    /// <param name="totalBytes">Total number of bytes.</param>
    public ByteSize(long totalBytes)
    {
        if (totalBytes == long.MinValue)
        {
            throw new ArgumentOutOfRangeException(nameof(totalBytes), "Value cannot be long.MinValue due to Math.Abs limitations.");
        }

        this._totalBytes = totalBytes;

        int sign = totalBytes < 0 ? -1 : 1;
        long remaining = Math.Abs(totalBytes);

        var pb = remaining / PB2B;
        remaining -= pb * PB2B;

        var tb = remaining / TB2B;
        remaining -= tb * TB2B;

        var gb = remaining / GB2B;
        remaining -= gb * GB2B;

        var mb = remaining / MB2B;
        remaining -= mb * MB2B;

        var kb = remaining / KB2B;
        remaining -= kb * KB2B;

        Bytes = (int)remaining * sign;

        PetaBytes = (int)pb * sign;
        TeraBytes = (int)tb * sign;
        GigaBytes = (int)gb * sign;
        MegaBytes = (int)mb * sign;
        KiloBytes = (int)kb * sign;
    }

    public static implicit operator ByteSize(long bytes)
    {
        return new ByteSize(bytes);
    }

    public static ByteSize operator +(ByteSize first, ByteSize second)
    {
        return new ByteSize(first._totalBytes + second._totalBytes);
    }
    /// <summary>Performs subtraction of two ByteSize instances.<br/>
    /// Note that this operator can produce ByteSize with negative number of bytes.</summary>
    /// <param name="first">First subtraction element.</param>
    /// <param name="second">Second subtraction element.</param>
    /// <returns>ByteSize with total number of bytes equal to <paramref name="first"/>.totalBytes-<paramref name="second"/>.totalBytes.</returns>
    public static ByteSize operator -(ByteSize first, ByteSize second)
    {
        return new ByteSize(first._totalBytes - second._totalBytes);
    }

    public static bool operator ==(ByteSize first, ByteSize second)
    {
        return first._totalBytes == second._totalBytes;
    }
    public static bool operator !=(ByteSize first, ByteSize second)
    {
        return first._totalBytes != second._totalBytes;
    }
    public static bool operator >(ByteSize first, ByteSize second)
    {
        return first._totalBytes > second._totalBytes;
    }
    public static bool operator <(ByteSize first, ByteSize second)
    {
        return first._totalBytes < second._totalBytes;
    }
    public static bool operator >=(ByteSize first, ByteSize second)
    {
        return first._totalBytes >= second._totalBytes;
    }
    public static bool operator <=(ByteSize first, ByteSize second)
    {
        return first._totalBytes <= second._totalBytes;
    }

    /// <summary>Performs safe subtraction of two ByteSize instances. Unlike with the minus operator, this method won't produce ByteSize with negative number of bytes. Value will be clamped to 0 bytes instead.</summary>
    /// <param name="first">First subtraction element.</param>
    /// <param name="second">Second subtraction element.</param>
    /// <returns>ByteSize with total number of bytes equal to <paramref name="first"/>.totalBytes-<paramref name="second"/>.totalBytes.</returns>
    public static ByteSize SubtractSafe(ByteSize first, ByteSize second)
    {
        return first.SubtractSafe(second);
    }
    /// <summary>Performs safe subtraction of two ByteSize instances. Unlike with the minus operator, this method won't produce ByteSize with negative number of bytes. Value will be clamped to 0 bytes instead.</summary>
    /// <param name="other">Second subtraction element.</param>
    /// <returns>ByteSize with total number of bytes equal to this.totalBytes-<paramref name="other"/>.totalBytes.</returns>
    public readonly ByteSize SubtractSafe(ByteSize other)
    {
        long bytes = this._totalBytes - other._totalBytes;
        return new ByteSize(bytes >= 0 ? bytes : 0);
    }

    /// <summary>Tries to convert string representation of ByteSize to object of that type.</summary>
    /// <param name="value">String to parse.</param>
    /// <param name="formatProvider">Format provider used to parse values.</param>
    /// <param name="result">ByteSize object or null if the conversion failed.</param>
    /// <returns>True if conversion was successfull, false otherwise.</returns>
    public static bool TryParse(string value, out ByteSize? result, IFormatProvider? formatProvider = null)
    {
        try
        {
            result = Parse(value, formatProvider);
        }
        catch
        {
            result = null;
            return false;
        }
        return true;
    }
    /// <summary>Converts string representation of ByteSize to object of that type.</summary>
    /// <param name="value">String to parse.</param>
    /// <param name="formatProvider">Format provider used to parse values.</param>
    /// <returns>Converted ByteSize object.</returns>
    public static ByteSize Parse(string value, IFormatProvider? formatProvider = null)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            throw new ArgumentException("Value cannot be null or whitespace.", nameof(value));
        }
        formatProvider ??= System.Globalization.CultureInfo.InvariantCulture;

        value = value.Trim();
        int sign = value.StartsWith('-') ? -1 : 1;
        value = value.TrimStart('-').TrimStart();

        long totalBytes = 0;
        string[] valueParts = value.Split(' ', StringSplitOptions.RemoveEmptyEntries);

        foreach (var part in valueParts)
        {
            totalBytes += ParseBytes(part, formatProvider);
        }
        return new ByteSize(totalBytes * sign);

        static long ParseBytes(string value, IFormatProvider? formatProvider)
        {
            if (string.IsNullOrWhiteSpace(value)) return 0;

            value = value.Trim();
            if (value.Length > 1)
            {
                // Reject bit units like "Mb"
                if (char.IsUpper(value[^2]) && value[^1] == 'b')
                {
                    throw new FormatException($"Bit units are unsupported: \"{value}\". Use byte units (B, KB, MB, GB, TB, PB) instead.");
                }

                var unitString = value[^2..].ToUpperInvariant();

                switch (unitString)
                {
                    case "PB":
                        return (long)(double.Parse(value[..^2], formatProvider) * PB2B);
                    case "TB":
                        return (long)(double.Parse(value[..^2], formatProvider) * TB2B);
                    case "GB":
                        return (long)(double.Parse(value[..^2], formatProvider) * GB2B);
                    case "MB":
                        return (long)(double.Parse(value[..^2], formatProvider) * MB2B);
                    case "KB":
                        return (long)(double.Parse(value[..^2], formatProvider) * KB2B);
                }

                if (unitString[1] == 'B')
                {
                    return (long)(double.Parse(value[..^1], formatProvider));
                }
            }
            throw new FormatException($"Unknown unit in value: \"{value}\". Supported units: B, KB, MB, GB, TB, PB.");
        }
    }

    public override readonly bool Equals(object? obj)
    {
        return obj is ByteSize size && Equals(size);
    }
    public readonly bool Equals(ByteSize other)
    {
        return _totalBytes == other._totalBytes;
    }

    public override readonly int GetHashCode()
    {
        return 1106757244 + _totalBytes.GetHashCode();
    }

    /// <summary>Formats the value of the current instance using the specified format.</summary>
    /// <param name="format">The format to use or a null reference to use the default format.<br/>
    /// Supported formats:
    /// <list type="bullet">
    /// <item>G - grouped units, e.g. "16MB 32KB 64B" (default format)</item>
    /// <item>B - total bytes, e.g. "16810048B"</item>
    /// <item>F - floating-point using largest applicable unit, e.g. "16,03MB"; supports precision via F0-F9</item>
    /// <item>KB, MB, GB, TB, PB - force specific unit, e.g. "16416,06KB"; supports precision via KB0-KB9</item>
    /// </list>
    /// </param>
    /// <param name="allowNegative">Should negative value of ByteSize be outputed normally with sign. By default negative ByteSize returns "&lt; 0B" string.</param>
    /// <returns>The value of the current instance in the specified format.</returns>
    public readonly string ToString(string? format, bool allowNegative = false)
    {
        return ToString(format, System.Globalization.CultureInfo.InvariantCulture, allowNegative);
    }
    /// <summary>Formats the value of the current instance using the specified format.</summary>
    /// <param name="format">The format to use or a null reference to use the default format.<br/>
    /// Supported formats:
    /// <list type="bullet">
    /// <item>G - grouped units, e.g. "16MB 32KB 64B" (default format)</item>
    /// <item>B - total bytes, e.g. "16810048B"</item>
    /// <item>F - floating-point using largest applicable unit, e.g. "16,03MB"; supports precision via F0-F9</item>
    /// <item>KB, MB, GB, TB, PB - force specific unit, e.g. "16416,06KB"; supports precision via KB0-KB9</item>
    /// </list>
    /// </param>
    /// <param name="formatProvider">Format provider used to display values.</param>
    /// <returns>The value of the current instance in the specified format.</returns>
    public readonly string ToString(string? format, IFormatProvider? formatProvider)
    {
        return ToString(format, formatProvider, allowNegative: false);
    }
    /// <summary>Formats the value of the current instance using the specified format.</summary>
    /// <param name="format">The format to use or a null reference to use the default format.<br/>
    /// Supported formats:
    /// <list type="bullet">
    /// <item>G - grouped units, e.g. "16MB 32KB 64B" (default format)</item>
    /// <item>B - total bytes, e.g. "16810048B"</item>
    /// <item>F - floating-point using largest applicable unit, e.g. "16,03MB"; supports precision via F0-F9</item>
    /// <item>KB, MB, GB, TB, PB - force specific unit, e.g. "16416,06KB"; supports precision via KB0-KB9</item>
    /// </list>
    /// </param>
    /// <param name="formatProvider">Format provider used to display values.</param>
    /// <param name="allowNegative">Should negative value of ByteSize be outputed normally with sign. By default negative ByteSize returns "&lt; 0B" string.</param>
    /// <returns>The value of the current instance in the specified format.</returns>
    public readonly string ToString(string? format, IFormatProvider? formatProvider, bool allowNegative)
    {
        if (_totalBytes <= 0) return ToString(allowNegative);
        format = format?.ToUpperInvariant() ?? "G";

        if (string.Equals(format, "G"))
        {
            return ToString(allowNegative);
        }
        else if (string.Equals(format, "B"))
        {
            return $"{TotalBytes}B";
        }
        else if (string.Equals(format, "F") || (format.Length == 2 && format[0] == 'F' && char.IsDigit(format[1])))
        {
            return ToStringAsFloat(format, formatProvider);
        }
        else
        {
            string floatFormat = format.Length == 3 ? $"F{format[2]}" : "F2";

            if (string.Equals(format, "KB") || (format.Length == 3 && format[0] == 'K' && format[1] == 'B' && char.IsDigit(format[2])))
            {
                return $"{TotalKiloBytes.ToString(floatFormat, formatProvider)}KB";
            }
            else if (string.Equals(format, "MB") || (format.Length == 3 && format[0] == 'M' && format[1] == 'B' && char.IsDigit(format[2])))
            {
                return $"{TotalMegaBytes.ToString(floatFormat, formatProvider)}MB";
            }
            else if (string.Equals(format, "GB") || (format.Length == 3 && format[0] == 'G' && format[1] == 'B' && char.IsDigit(format[2])))
            {
                return $"{TotalGigaBytes.ToString(floatFormat, formatProvider)}GB";
            }
            else if (string.Equals(format, "TB") || (format.Length == 3 && format[0] == 'T' && format[1] == 'B' && char.IsDigit(format[2])))
            {
                return $"{TotalTeraBytes.ToString(floatFormat, formatProvider)}TB";
            }
            else if (string.Equals(format, "PB") || (format.Length == 3 && format[0] == 'P' && format[1] == 'B' && char.IsDigit(format[2])))
            {
                return $"{TotalPetaBytes.ToString(floatFormat, formatProvider)}PB";
            }
        }
        return ToString(allowNegative); // fallback
    }
    private readonly string ToStringAsFloat(string floatFormat, IFormatProvider? formatProvider)
    {
        if (PetaBytes > 0) return $"{TotalPetaBytes.ToString(floatFormat, formatProvider)}PB";
        if (TeraBytes > 0) return $"{TotalTeraBytes.ToString(floatFormat, formatProvider)}TB";
        if (GigaBytes > 0) return $"{TotalGigaBytes.ToString(floatFormat, formatProvider)}GB";
        if (MegaBytes > 0) return $"{TotalMegaBytes.ToString(floatFormat, formatProvider)}MB";
        if (KiloBytes > 0) return $"{TotalKiloBytes.ToString(floatFormat, formatProvider)}KB";
        return $"{TotalBytes.ToString(floatFormat, formatProvider)}B";
    }

    /// <summary>Formats the value of the current instance using the default format (grouped units, e.g. "16MB 32KB 64B").</summary>
    /// <returns>This instance as string.</returns>
    public override readonly string ToString()
    {
        return ToString(allowNegative: false);
    }
    /// <summary>Formats the value of the current instance using the default format (grouped units, e.g. "16MB 32KB 64B").</summary>
    /// <param name="allowNegative">Should negative value of ByteSize be outputed normally with sign. By default negative ByteSize returns "&lt; 0B" string.</param>
    /// <returns>This instance as string.</returns>
    public readonly string ToString(bool allowNegative)
    {
        if (_totalBytes == 0) return "0B";
        if (allowNegative && TotalBytes < 0)
        {
            var pb = Math.Abs(PetaBytes);
            var tb = Math.Abs(TeraBytes);
            var gb = Math.Abs(GigaBytes);
            var mb = Math.Abs(MegaBytes);
            var kb = Math.Abs(KiloBytes);
            var b = Math.Abs(Bytes);
            return $"- {(pb > 0 ? pb + "PB " : string.Empty)}{(tb > 0 ? tb + "TB " : string.Empty)}{(gb > 0 ? gb + "GB " : string.Empty)}{(mb > 0 ? mb + "MB " : string.Empty)}{(kb > 0 ? kb + "KB " : string.Empty)}{(b > 0 ? b + "B " : string.Empty)}";
        }
        else
        {
            if (_totalBytes < 0) return "< 0B";
            return $"{(PetaBytes > 0 ? PetaBytes + "PB " : string.Empty)}{(TeraBytes > 0 ? TeraBytes + "TB " : string.Empty)}{(GigaBytes > 0 ? GigaBytes + "GB " : string.Empty)}{(MegaBytes > 0 ? MegaBytes + "MB " : string.Empty)}{(KiloBytes > 0 ? KiloBytes + "KB " : string.Empty)}{(Bytes > 0 ? Bytes + "B " : string.Empty)}";
        }
    }
}
