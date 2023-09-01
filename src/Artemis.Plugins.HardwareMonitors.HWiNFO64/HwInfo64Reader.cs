using System;
using System.IO.MemoryMappedFiles;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace Artemis.Plugins.HardwareMonitors.HWiNFO64;

internal sealed class HwInfo64Reader : IDisposable
{
    private const string SharedMemory = @"Global\HWiNFO_SENS_SM2";
    private readonly MemoryMappedFile _file;
    private readonly MemoryMappedViewAccessor _root;
    private readonly MemoryMappedViewAccessor _hardwares;
    private readonly MemoryMappedViewAccessor _sensors;
    
    public HwInfo64Reader()
    {
        _file = MemoryMappedFile.OpenExisting(SharedMemory, MemoryMappedFileRights.Read);
        _root = _file.CreateViewAccessor(0, Unsafe.SizeOf<HwInfoRoot>(), MemoryMappedFileAccess.Read);
        var data = ReadRoot();
        _hardwares = _file.CreateViewAccessor(data.HardwareSectionOffset, data.HardwareSize * data.HardwareCount, MemoryMappedFileAccess.Read);
        _sensors = _file.CreateViewAccessor(data.SensorSectionOffset, data.SensorSize * data.SensorCount, MemoryMappedFileAccess.Read);
    }


    public void Dispose()
    {
        _sensors.Dispose();
        _hardwares.Dispose();
        _root.Dispose();
        _file.Dispose();
    }

    public HwInfoRoot ReadRoot()
    {
        if (_root.SafeMemoryMappedViewHandle.IsClosed)
            throw new ObjectDisposedException(nameof(HwInfo64Reader));
        
        _root.Read<HwInfoRoot>(0, out var root);
        
        return root;
    }
    
    public HwInfoHardware ReadHardware(int index)
    {
        if (_hardwares.SafeMemoryMappedViewHandle.IsClosed)
            throw new ObjectDisposedException(nameof(HwInfo64Reader));
        
        var size = Unsafe.SizeOf<HwInfoHardware>();
        
        _hardwares.Read<HwInfoHardware>(index * size, out var hardware);
        
        return hardware;
    }
    
    public void ReadAllSensors(HwInfoSensor[] sensors)
    {
        if (_sensors.SafeMemoryMappedViewHandle.IsClosed)
            throw new ObjectDisposedException(nameof(HwInfo64Reader));
        
        _sensors.ReadArray(0, sensors, 0, sensors.Length);
    }
}