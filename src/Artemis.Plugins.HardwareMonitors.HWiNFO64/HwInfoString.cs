using System;
using System.Runtime.InteropServices;
using System.Text;
using UnmanagedArrayGenerator;

namespace Artemis.Plugins.HardwareMonitors.HWiNFO64;

[UnmanagedArray(typeof(byte), 128)]
public readonly partial struct HwInfoString128 { }

[UnmanagedArray(typeof(byte), 16)]
public readonly partial struct HwInfoString16 { }

[StructLayout(LayoutKind.Sequential, Pack = 1)]
public readonly struct HwInfoString128Ascii
{
    private readonly HwInfoString128 _str;
        
    public override string ToString()
    {
        var sb = new StringBuilder();

        for (var i = 0; i < 128; i++)
        {
            //ascii only
            var c = _str[i];
            if (c == 0)
                break;

            sb.Append((char)c);
        }

        return sb.ToString();
    }

    public static implicit operator string(HwInfoString128Ascii str)
    {
        return str.ToString();
    }
}

[StructLayout(LayoutKind.Sequential, Pack = 1)]
public readonly struct HwInfoString128Utf8
{
    private readonly HwInfoString128 _str;
    
    public override string ToString()
    {
        Span<byte> bytes = stackalloc byte[128];
        
        for (var i = 0; i < 128; i++)
        {
            bytes[i] = _str[i];
        }
        
        var endOfString = bytes.LastIndexOfAnyExcept((byte)0);
        //i'm assuming any contiguous zeroes after are null-terminators.
        //zeroes in the middle of the string are valid utf8, so we can't use that.
        return Encoding.UTF8.GetString(bytes[..(endOfString + 1)]);
    }
    
    public static implicit operator string(HwInfoString128Utf8 str)
    {
        return str.ToString();
    }
}

[StructLayout(LayoutKind.Sequential, Pack = 1)]
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

[StructLayout(LayoutKind.Sequential, Pack = 1)]
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