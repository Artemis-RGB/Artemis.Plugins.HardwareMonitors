using System;
using System.Runtime.InteropServices;
using System.Text;

namespace Artemis.Plugins.HardwareMonitors.HWiNFO64;

[StructLayout(LayoutKind.Sequential, Pack = 1)]
public readonly struct HwInfoString16
{
    public readonly byte Child00;
    public readonly byte Child01;
    public readonly byte Child02;
    public readonly byte Child03;
    public readonly byte Child04;
    public readonly byte Child05;
    public readonly byte Child06;
    public readonly byte Child07;
    public readonly byte Child08;
    public readonly byte Child09;
    public readonly byte Child10;
    public readonly byte Child11;
    public readonly byte Child12;
    public readonly byte Child13;
    public readonly byte Child14;
    public readonly byte Child15;

    public byte this[int index] => index switch
    {
        0 => Child00,
        1 => Child01,
        2 => Child02,
        3 => Child03,
        4 => Child04,
        5 => Child05,
        6 => Child06,
        7 => Child07,
        8 => Child08,
        9 => Child09,
        10 => Child10,
        11 => Child11,
        12 => Child12,
        13 => Child13,
        14 => Child14,
        15 => Child15,
        _ => throw new IndexOutOfRangeException()
    };
}

public readonly struct HwInfoString16Ascii
{
    private readonly HwInfoString16 _str;
        
    public override string ToString()
    {
        var sb = new StringBuilder();

        for (var i = 0; i < 16; i++)
        {
            //ascii only
            var c = _str[i];
            if (c == 0)
                break;

            sb.Append((char)c);
        }

        return sb.ToString();
    }

    public static implicit operator string(HwInfoString16Ascii str)
    {
        return str.ToString();
    }
}

public readonly struct HwInfoString16Utf8
{
    private readonly HwInfoString16 _str;
    
    public override string ToString()
    {
        Span<byte> bytes = stackalloc byte[16];
        
        for (var i = 0; i < 16; i++)
        {
            bytes[i] = _str[i];
        }
        
        var endOfString = bytes.LastIndexOfAnyExcept((byte)0);
        //i'm assuming any contiguous zeroes after are null-terminators.
        //zeroes in the middle of the string are valid utf8, so we can't use that.
        return Encoding.UTF8.GetString(bytes[..(endOfString + 1)]);
    }
    
    public static implicit operator string(HwInfoString16Utf8 str)
    {
        return str.ToString();
    }
}