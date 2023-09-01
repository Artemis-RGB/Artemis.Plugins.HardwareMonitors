using Artemis.Core.Modules;

namespace Artemis.Plugins.HardwareMonitors.HWiNFO64.DataModels;

public class HwInfoDataModel : DataModel { }
public class HardwareDataModel : DataModel { }
public class SensorTypeDataModel : DataModel { }

public class SensorDataModel : DataModel
{
    public double CurrentValue { get; set; }
    public double Minimum { get; set; }
    public double Maximum { get; set; }
    public double Average { get; set; }
}