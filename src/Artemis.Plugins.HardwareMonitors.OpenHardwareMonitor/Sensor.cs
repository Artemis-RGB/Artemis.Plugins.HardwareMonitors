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
            SensorType sensorType;
            if (!Enum.TryParse<SensorType>((string)obj[nameof(SensorType)], out sensorType))
            {
                sensorType = SensorType.Other;
            }

            return new Sensor
            {
                Identifier = (string)obj[nameof(Identifier)],
                Min = (float)obj[nameof(Min)],
                Max = (float)obj[nameof(Max)],
                Value = (float)obj[nameof(Value)],
                Name = (string)obj[nameof(Name)],
                Parent = (string)obj[nameof(Parent)],
                SensorType = sensorType
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
        Voltage, // V
        Current, // A
        Power, // W
        Clock, // MHz
        Temperature, // °C
        Load, // %
        Frequency, // Hz
        Fan, // RPM
        Flow, // L/h
        Control, // %
        Level, // %
        Factor, // 1
        Data, // GB = 2^30 Bytes
        SmallData, // MB = 2^20 Bytes
        Throughput, // B/s
        TimeSpan, // Seconds 
        Energy, // milliwatt-hour (mWh)
        Noise, // dBA
        Conductivity, // µS/cm
        Other
    }
}
