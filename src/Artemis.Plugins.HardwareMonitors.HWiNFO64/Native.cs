using System.Runtime.InteropServices;

namespace Artemis.Plugins.HardwareMonitors.HWiNFO64;

[StructLayout(LayoutKind.Sequential, Pack = 1)]
internal readonly struct HwInfoRoot
{
    internal readonly uint Signature;
    internal readonly uint Version;
    internal readonly uint Revision;
    internal readonly long PollTime;

    internal readonly uint HardwareSectionOffset;
    internal readonly uint HardwareSize;
    internal readonly uint HardwareCount;

    internal readonly uint SensorSectionOffset;
    internal readonly uint SensorSize;
    internal readonly uint SensorCount;

    internal readonly uint PollingPeriod;
}

[StructLayout(LayoutKind.Sequential, Pack = 1)]
internal readonly struct HwInfoHardware
{
    internal readonly uint HardwareId;
    internal readonly uint HardwareInstance;
    internal readonly HwInfoString128Ascii NameOriginal;
    internal readonly HwInfoString128Ascii NameCustom;
    
    internal readonly HwInfoString128Utf8 NameCustomUtf8;
    
    internal string GetId()
    {
        return $"{HardwareId}-{HardwareInstance}";
    }
}

[StructLayout(LayoutKind.Sequential, Pack = 1)]
internal readonly struct HwInfoSensor
{
    internal readonly HwInfoSensorType SensorType;
    internal readonly uint ParentIndex;
    internal readonly uint SensorId;
    internal readonly HwInfoString128Ascii LabelOriginal;
    internal readonly HwInfoString128Ascii LabelCustom;
    internal readonly HwInfoString16Ascii SensorUnit;
    internal readonly double Value;
    internal readonly double ValueMin;
    internal readonly double ValueMax;
    internal readonly double ValueAvg;

    internal readonly HwInfoString128Utf8 LabelCustomUtf8;
    internal readonly HwInfoString16Utf8 SensorUnitUtf8;

    internal ulong Id => (ParentIndex * 100000000000ul) + SensorId;
}

public enum HwInfoSensorType
{
    None = 0,
    Temperature,
    Volt,
    Fan,
    Current,
    Power,
    Clock,
    Usage,
    Other
};