using System;
using System.Collections.Generic;
using System.Management;

namespace Artemis.Plugins.HardwareMonitors.OpenHardwareMonitor
{
    public class Sensor : IComparable
    {
        public string Identifier { get; set; }
        public float Min { get; set; }
        public float Max { get; set; }
        public float Value { get; set; }
        public string Name { get; set; }
        public string Parent { get; set; }
        public SensorType SensorType { get; set; }

        public static Sensor FromManagementObject(ManagementBaseObject obj)
        {
            return new Sensor
            {
                Identifier = (string)obj[nameof(Identifier)],
                Min = (float)obj[nameof(Min)],
                Max = (float)obj[nameof(Max)],
                Value = (float)obj[nameof(Value)],
                Name = (string)obj[nameof(Name)],
                Parent = (string)obj[nameof(Parent)],
                SensorType = Enum.Parse<SensorType>((string)obj[nameof(SensorType)])
            };
        }

        public static Sensor FromManagementObjectFast(ManagementBaseObject obj)
        {
            return new Sensor
            {
                Identifier = (string)obj[nameof(Identifier)],
                Min = (float)obj[nameof(Min)],
                Max = (float)obj[nameof(Max)],
                Value = (float)obj[nameof(Value)]
            };
        }

        public static List<Sensor> FromCollection(ManagementObjectCollection collection)
        {
            List<Sensor> list = new(collection.Count);

            foreach (ManagementBaseObject obj in collection)
            {
                Sensor sensor = FromManagementObject(obj);
                if (sensor.SensorType != SensorType.Control)
                {
                    list.Add(sensor);
                }
            }

            return list;
        }

        public static IEnumerable<Sensor> FromCollectionFast(ManagementObjectCollection collection)
        {
            foreach (ManagementBaseObject item in collection)
            {
                yield return FromManagementObjectFast(item);
            }
        }

        public override string ToString()
        {
            return $"{Identifier} : {Value}";
        }

        public int CompareTo(object other)
        {
            return Identifier.CompareTo(((Sensor)other).Identifier);
        }
    }

    public enum SensorType
    {
        Temperature,
        Voltage,
        Level,
        SmallData,
        Load,
        Data,
        Power,
        Fan,
        Throughput,
        Factor,
        Control,
        Clock
    }
}
