using System.Collections.Generic;
using System.Management;

namespace Artemis.Plugins.HardwareMonitors.OpenHardwareMonitor
{
    public class Hardware
    {
        public string Identifier { get; set; }
        public string Name { get; set; }
        public HardwareType HardwareType { get; set; }

        public static Hardware FromManagementObject(ManagementBaseObject obj)
        {
            return new Hardware
            {
                Identifier = (string)obj[nameof(Identifier)],
                Name = (string)obj[nameof(Name)],
                HardwareType = GetHardwareType((string)obj[nameof(HardwareType)]),
            };
        }

        public static List<Hardware> FromCollection(ManagementObjectCollection collection)
        {
            List<Hardware> list = new(collection.Count);

            foreach (ManagementBaseObject obj in collection)
                list.Add(FromManagementObject(obj));

            return list;
        }

        private static HardwareType GetHardwareType(string name)
        {
            return name.ToLower() switch
            {
                "motherboard" => HardwareType.Motherboard,
                "mainboard" => HardwareType.Motherboard,
                "superio" => HardwareType.SuperIO,
                "cpu" => HardwareType.Cpu,
                "memory" => HardwareType.Memory,
                "ram" => HardwareType.Memory,
                "gpunvidia" => HardwareType.Gpu,
                "gpuamd" => HardwareType.Gpu,
                "storage" => HardwareType.Storage,
                "hdd" => HardwareType.Storage,
                "network" => HardwareType.Network,
                "cooler" => HardwareType.Cooler,
                _ => HardwareType.Unknown
            };
        }
    }

    public enum HardwareType
    {
        Cpu,
        Gpu,
        Memory,
        Motherboard,
        SuperIO,
        Storage,
        Network,
        Cooler,
        Unknown
    }
}
