using Artemis.Core.Modules;

namespace Artemis.Plugins.HardwareMonitors.OpenHardwareMonitor
{
    public class HardwareMonitorDataModel : DataModel
    {
    }

    public class HardwareDynamicDataModel : DataModel
    {
    }

    public class SensorTypeDynamicDataModel : DataModel
    {
    }

    public class SensorDynamicDataModel : DataModel
    {
        [DataModelIgnore] internal string Identifier { get; set; }

        public virtual float CurrentValue { get; set; }
        public virtual float Minimum { get; set; }
        public virtual float Maximum { get; set; }

        public SensorDynamicDataModel(string id)
        {
            Identifier = id;
        }
    }

    public class VoltageDynamicDataModel : SensorDynamicDataModel
    {
        [DataModelProperty(Affix = "V")] public override float CurrentValue { get; set; }
        [DataModelProperty(Affix = "V")] public override float Minimum { get; set; }
        [DataModelProperty(Affix = "V")] public override float Maximum { get; set; }

        public VoltageDynamicDataModel(string id) : base(id)
        {
        }
    }

    public class CurrentDynamicDataModel : SensorDynamicDataModel
    {
        [DataModelProperty(Affix = "A")] public override float CurrentValue { get; set; }
        [DataModelProperty(Affix = "A")] public override float Minimum { get; set; }
        [DataModelProperty(Affix = "A")] public override float Maximum { get; set; }

        public CurrentDynamicDataModel(string id) : base(id)
        {
        }
    }

    public class PowerDynamicDataModel : SensorDynamicDataModel
    {
        [DataModelProperty(Affix = "W")] public override float CurrentValue { get; set; }
        [DataModelProperty(Affix = "W")] public override float Minimum { get; set; }
        [DataModelProperty(Affix = "W")] public override float Maximum { get; set; }

        public PowerDynamicDataModel(string id) : base(id)
        {
        }
    }

    public class ClockDynamicDataModel : SensorDynamicDataModel
    {
        [DataModelProperty(Affix = "MHz")] public override float CurrentValue { get; set; }
        [DataModelProperty(Affix = "MHz")] public override float Minimum { get; set; }
        [DataModelProperty(Affix = "MHz")] public override float Maximum { get; set; }

        public ClockDynamicDataModel(string id) : base(id)
        {
        }
    }
    
    public class TemperatureDynamicDataModel : SensorDynamicDataModel
    {
        [DataModelProperty(Affix = "°C")] public override float CurrentValue { get; set; }
        [DataModelProperty(Affix = "°C")] public override float Minimum { get; set; }
        [DataModelProperty(Affix = "°C")] public override float Maximum { get; set; }

        public TemperatureDynamicDataModel(string id) : base(id)
        {
        }
    }

    public class PercentageDynamicDataModel : SensorDynamicDataModel
    {
        [DataModelProperty(Affix = "%")] public override float CurrentValue { get; set; }
        [DataModelProperty(Affix = "%")] public override float Minimum { get; set; }
        [DataModelProperty(Affix = "%")] public override float Maximum { get; set; }

        public PercentageDynamicDataModel(string id) : base(id)
        {
        }
    }

    public class FrequencyDynamicDataModel : SensorDynamicDataModel
    {
        [DataModelProperty(Affix = "Hz")] public override float CurrentValue { get; set; }
        [DataModelProperty(Affix = "Hz")] public override float Minimum { get; set; }
        [DataModelProperty(Affix = "Hz")] public override float Maximum { get; set; }

        public FrequencyDynamicDataModel(string id) : base(id)
        {
        }
    }
    
    public class FanDynamicDataModel : SensorDynamicDataModel
    {
        [DataModelProperty(Affix = "RPM")] public override float CurrentValue { get; set; }
        [DataModelProperty(Affix = "RPM")] public override float Minimum { get; set; }
        [DataModelProperty(Affix = "RPM")] public override float Maximum { get; set; }

        public FanDynamicDataModel(string id) : base(id)
        {
        }
    }
    
    public class FlowDynamicDataModel : SensorDynamicDataModel
    {
        [DataModelProperty(Affix = "L/h")] public override float CurrentValue { get; set; }
        [DataModelProperty(Affix = "L/h")] public override float Minimum { get; set; }
        [DataModelProperty(Affix = "L/h")] public override float Maximum { get; set; }

        public FlowDynamicDataModel(string id) : base(id)
        {
        }
    }
    
    public class SmallDataDynamicDataModel : SensorDynamicDataModel
    {
        [DataModelProperty(Affix = "MB")] public override float CurrentValue { get; set; }
        [DataModelProperty(Affix = "MB")] public override float Minimum { get; set; }
        [DataModelProperty(Affix = "MB")] public override float Maximum { get; set; }

        public SmallDataDynamicDataModel(string id) : base(id)
        {
        }
    }

    public class BigDataDynamicDataModel : SensorDynamicDataModel
    {
        [DataModelProperty(Affix = "GB")] public override float CurrentValue { get; set; }
        [DataModelProperty(Affix = "GB")] public override float Minimum { get; set; }
        [DataModelProperty(Affix = "GB")] public override float Maximum { get; set; }

        public BigDataDynamicDataModel(string id) : base(id)
        {
        }
    }

    public class ThroughputDynamicDataModel : SensorDynamicDataModel
    {
        [DataModelProperty(Affix = "B/s")] public override float CurrentValue { get; set; }
        [DataModelProperty(Affix = "B/s")] public override float Minimum { get; set; }
        [DataModelProperty(Affix = "B/s")] public override float Maximum { get; set; }

        public ThroughputDynamicDataModel(string id) : base(id)
        {
        }
    }

    public class TimeSpanDynamicDataModel : SensorDynamicDataModel
    {
        [DataModelProperty(Affix = "Seconds")] public override float CurrentValue { get; set; }
        [DataModelProperty(Affix = "Seconds")] public override float Minimum { get; set; }
        [DataModelProperty(Affix = "Seconds")] public override float Maximum { get; set; }

        public TimeSpanDynamicDataModel(string id) : base(id)
        {
        }
    }
    
    public class EnergyDynamicDataModel : SensorDynamicDataModel
    {
        [DataModelProperty(Affix = "mWh")] public override float CurrentValue { get; set; }
        [DataModelProperty(Affix = "mWh")] public override float Minimum { get; set; }
        [DataModelProperty(Affix = "mWh")] public override float Maximum { get; set; }

        public EnergyDynamicDataModel(string id) : base(id)
        {
        }
    }
    
    public class NoiseDynamicDataModel : SensorDynamicDataModel
    {
        [DataModelProperty(Affix = "dBA")] public override float CurrentValue { get; set; }
        [DataModelProperty(Affix = "dBA")] public override float Minimum { get; set; }
        [DataModelProperty(Affix = "dBA")] public override float Maximum { get; set; }

        public NoiseDynamicDataModel(string id) : base(id)
        {
        }
    }
}