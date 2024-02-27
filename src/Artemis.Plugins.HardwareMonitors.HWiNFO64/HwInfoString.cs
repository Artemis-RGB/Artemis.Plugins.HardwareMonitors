using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;

namespace Artemis.Plugins.HardwareMonitors.HWiNFO64;

[InlineArray(128)]
public struct HwInfoString128
{
    private byte _field;
}

[InlineArray(16)]
public struct HwInfoString16
{
    private byte _field;
}

[StructLayout(LayoutKind.Sequential, Pack = 1)]
public readonly struct HwInfoString128Ascii
{
    private readonly HwInfoString128 _str;
        
    public override string ToString()
    {
        var sb = new StringBuilder();

        foreach (ref readonly var c in _str)
        {
            //ascii only
            if (c == 0)
                break;

            sb.Append((char)c);
        }
        
        return sb.ToString();
    }

    public static implicit operator string(in HwInfoString128Ascii str) => str.ToString();
}

[StructLayout(LayoutKind.Sequential, Pack = 1)]
public readonly struct HwInfoString128Utf8
{
    private readonly HwInfoString128 _str;
    
    public override string ToString()
    {
        ReadOnlySpan<byte> bytes = _str;
        
        var endOfString = bytes.LastIndexOfAnyExcept((byte)0);
        //i'm assuming any contiguous zeroes after are null-terminators.
        //zeroes in the middle of the string are valid utf8, so we can't use that.
        return Encoding.UTF8.GetString(bytes[..(endOfString + 1)]);
    }
    
    public static implicit operator string(in HwInfoString128Utf8 str) => str.ToString();
}

[StructLayout(LayoutKind.Sequential, Pack = 1)]
public readonly struct HwInfoString16Ascii
{
    private readonly HwInfoString16 _str;
        
    public override string ToString()
    {
        var sb = new StringBuilder();
        
        foreach (ref readonly var c in _str)
        {
            //ascii only
            if (c == 0)
                break;

            sb.Append((char)c);
        }

        return sb.ToString();
    }

    public static implicit operator string(in HwInfoString16Ascii str) => str.ToString();
}

[StructLayout(LayoutKind.Sequential, Pack = 1)]
public readonly struct HwInfoString16Utf8
{
    private readonly HwInfoString16 _str;
    
    public override string ToString()
    {
        ReadOnlySpan<byte> bytes = _str;
        
        var endOfString = bytes.LastIndexOfAnyExcept((byte)0);
        //i'm assuming any contiguous zeroes after are null-terminators.
        //zeroes in the middle of the string are valid utf8, so we can't use that.
        return Encoding.UTF8.GetString(bytes[..(endOfString + 1)]);
    }
    
    public static implicit operator string(in HwInfoString16Utf8 str) => str.ToString();
}