using System;
using System.Runtime.InteropServices;
using System.Text;

namespace Artemis.Plugins.HardwareMonitors.HWiNFO64;

[StructLayout(LayoutKind.Sequential, Pack = 1)]
public readonly struct HwInfoString128
{
    public readonly byte Child000;
    public readonly byte Child001;
    public readonly byte Child002;
    public readonly byte Child003;
    public readonly byte Child004;
    public readonly byte Child005;
    public readonly byte Child006;
    public readonly byte Child007;
    public readonly byte Child008;
    public readonly byte Child009;
    public readonly byte Child010;
    public readonly byte Child011;
    public readonly byte Child012;
    public readonly byte Child013;
    public readonly byte Child014;
    public readonly byte Child015;
    public readonly byte Child016;
    public readonly byte Child017;
    public readonly byte Child018;
    public readonly byte Child019;
    public readonly byte Child020;
    public readonly byte Child021;
    public readonly byte Child022;
    public readonly byte Child023;
    public readonly byte Child024;
    public readonly byte Child025;
    public readonly byte Child026;
    public readonly byte Child027;
    public readonly byte Child028;
    public readonly byte Child029;
    public readonly byte Child030;
    public readonly byte Child031;
    public readonly byte Child032;
    public readonly byte Child033;
    public readonly byte Child034;
    public readonly byte Child035;
    public readonly byte Child036;
    public readonly byte Child037;
    public readonly byte Child038;
    public readonly byte Child039;
    public readonly byte Child040;
    public readonly byte Child041;
    public readonly byte Child042;
    public readonly byte Child043;
    public readonly byte Child044;
    public readonly byte Child045;
    public readonly byte Child046;
    public readonly byte Child047;
    public readonly byte Child048;
    public readonly byte Child049;
    public readonly byte Child050;
    public readonly byte Child051;
    public readonly byte Child052;
    public readonly byte Child053;
    public readonly byte Child054;
    public readonly byte Child055;
    public readonly byte Child056;
    public readonly byte Child057;
    public readonly byte Child058;
    public readonly byte Child059;
    public readonly byte Child060;
    public readonly byte Child061;
    public readonly byte Child062;
    public readonly byte Child063;
    public readonly byte Child064;
    public readonly byte Child065;
    public readonly byte Child066;
    public readonly byte Child067;
    public readonly byte Child068;
    public readonly byte Child069;
    public readonly byte Child070;
    public readonly byte Child071;
    public readonly byte Child072;
    public readonly byte Child073;
    public readonly byte Child074;
    public readonly byte Child075;
    public readonly byte Child076;
    public readonly byte Child077;
    public readonly byte Child078;
    public readonly byte Child079;
    public readonly byte Child080;
    public readonly byte Child081;
    public readonly byte Child082;
    public readonly byte Child083;
    public readonly byte Child084;
    public readonly byte Child085;
    public readonly byte Child086;
    public readonly byte Child087;
    public readonly byte Child088;
    public readonly byte Child089;
    public readonly byte Child090;
    public readonly byte Child091;
    public readonly byte Child092;
    public readonly byte Child093;
    public readonly byte Child094;
    public readonly byte Child095;
    public readonly byte Child096;
    public readonly byte Child097;
    public readonly byte Child098;
    public readonly byte Child099;
    public readonly byte Child100;
    public readonly byte Child101;
    public readonly byte Child102;
    public readonly byte Child103;
    public readonly byte Child104;
    public readonly byte Child105;
    public readonly byte Child106;
    public readonly byte Child107;
    public readonly byte Child108;
    public readonly byte Child109;
    public readonly byte Child110;
    public readonly byte Child111;
    public readonly byte Child112;
    public readonly byte Child113;
    public readonly byte Child114;
    public readonly byte Child115;
    public readonly byte Child116;
    public readonly byte Child117;
    public readonly byte Child118;
    public readonly byte Child119;
    public readonly byte Child120;
    public readonly byte Child121;
    public readonly byte Child122;
    public readonly byte Child123;
    public readonly byte Child124;
    public readonly byte Child125;
    public readonly byte Child126;
    public readonly byte Child127;

    public byte this[int index] => index switch
    {
        0 => Child000,
        1 => Child001,
        2 => Child002,
        3 => Child003,
        4 => Child004,
        5 => Child005,
        6 => Child006,
        7 => Child007,
        8 => Child008,
        9 => Child009,
        10 => Child010,
        11 => Child011,
        12 => Child012,
        13 => Child013,
        14 => Child014,
        15 => Child015,
        16 => Child016,
        17 => Child017,
        18 => Child018,
        19 => Child019,
        20 => Child020,
        21 => Child021,
        22 => Child022,
        23 => Child023,
        24 => Child024,
        25 => Child025,
        26 => Child026,
        27 => Child027,
        28 => Child028,
        29 => Child029,
        30 => Child030,
        31 => Child031,
        32 => Child032,
        33 => Child033,
        34 => Child034,
        35 => Child035,
        36 => Child036,
        37 => Child037,
        38 => Child038,
        39 => Child039,
        40 => Child040,
        41 => Child041,
        42 => Child042,
        43 => Child043,
        44 => Child044,
        45 => Child045,
        46 => Child046,
        47 => Child047,
        48 => Child048,
        49 => Child049,
        50 => Child050,
        51 => Child051,
        52 => Child052,
        53 => Child053,
        54 => Child054,
        55 => Child055,
        56 => Child056,
        57 => Child057,
        58 => Child058,
        59 => Child059,
        60 => Child060,
        61 => Child061,
        62 => Child062,
        63 => Child063,
        64 => Child064,
        65 => Child065,
        66 => Child066,
        67 => Child067,
        68 => Child068,
        69 => Child069,
        70 => Child070,
        71 => Child071,
        72 => Child072,
        73 => Child073,
        74 => Child074,
        75 => Child075,
        76 => Child076,
        77 => Child077,
        78 => Child078,
        79 => Child079,
        80 => Child080,
        81 => Child081,
        82 => Child082,
        83 => Child083,
        84 => Child084,
        85 => Child085,
        86 => Child086,
        87 => Child087,
        88 => Child088,
        89 => Child089,
        90 => Child090,
        91 => Child091,
        92 => Child092,
        93 => Child093,
        94 => Child094,
        95 => Child095,
        96 => Child096,
        97 => Child097,
        98 => Child098,
        99 => Child099,
        100 => Child100,
        101 => Child101,
        102 => Child102,
        103 => Child103,
        104 => Child104,
        105 => Child105,
        106 => Child106,
        107 => Child107,
        108 => Child108,
        109 => Child109,
        110 => Child110,
        111 => Child111,
        112 => Child112,
        113 => Child113,
        114 => Child114,
        115 => Child115,
        116 => Child116,
        117 => Child117,
        118 => Child118,
        119 => Child119,
        120 => Child120,
        121 => Child121,
        122 => Child122,
        123 => Child123,
        124 => Child124,
        125 => Child125,
        126 => Child126,
        127 => Child127,
        _ => throw new IndexOutOfRangeException()
    };

}

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