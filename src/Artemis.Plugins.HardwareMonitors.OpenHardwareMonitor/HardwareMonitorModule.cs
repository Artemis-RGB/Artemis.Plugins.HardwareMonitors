using System;
using System.Collections.Generic;
using System.Linq;
using System.Management;
using System.Threading;
using Artemis.Core;
using Artemis.Core.Modules;
using DryIoc.ImTools;
using Serilog;

namespace Artemis.Plugins.HardwareMonitors.OpenHardwareMonitor
{
    [PluginFeature(AlwaysEnabled = true)]
    public class HardwareMonitorModule : Module<HardwareMonitorDataModel>
    {
        private readonly ILogger _logger;

        private static readonly string[] Scopes = new[]
        {
            @"\\.\root\LibreHardwareMonitor",
            @"\\.\root\OpenHardwareMonitor"
        };

        private WmiUpdater _updater;
        private readonly Dictionary<string, DynamicChild<SensorDynamicDataModel>> _cache = new();

        public override List<IModuleActivationRequirement> ActivationRequirements { get; } = new()
        {
            new HardwareMonitorActivationRequirement(@"\\.\root\OpenHardwareMonitor", "OpenHardwareMonitor"),
            new HardwareMonitorActivationRequirement(@"\\.\root\LibreHardwareMonitor", "LibreHardwareMonitor")
        };

        public HardwareMonitorModule(ILogger logger)
        {
            _logger = logger;

            ActivationRequirementMode = ActivationRequirementType.Any;
            UpdateDuringActivationOverride = false;

            AddTimedUpdate(TimeSpan.FromMilliseconds(500), UpdateSensorsAndDataModel, nameof(UpdateSensorsAndDataModel));
        }

        public override void Enable() { }

        public override void Disable() { }

        public override void Update(double deltaTime) { }

        public override void ModuleActivated(bool isOverride)
        {
            _logger.Information("Activate OpenHardwareMonitor Plugin");

            if (isOverride)
                return;

            foreach (string scope in Scopes)
            {
                WmiUpdater updater;

                try
                {
                    updater = new WmiUpdater(scope);
                }
                catch
                {
                    _logger.Warning($"Could not connect to WMI scope: {scope}");

                    //if the connection to one of the scopes fails,
                    //ignore the exception and try the other one.
                    //this way both Open and Libre HardwareMonitors
                    //can be supported since only the name of
                    //scope differs
                    continue;
                }

                try
                {
                    var sensors = updater.FetchSensors();
                    var hardwares = updater.FetchHardwares();

                    if (sensors.Count == 0 || hardwares.Count == 0)
                    {
                        _logger.Warning($"Connected to WMI scope \"{scope}\" but it did not contain any data.");

                        updater.Dispose();

                        continue;
                    }

                    PopulateDynamicDataModels(hardwares, sensors);
                }
                catch (Exception e)
                {
                    updater.Dispose();

                    _logger.Error(e, "Error while populating dynamic data models");

                    return;
                }

                _logger.Information($"Successfully connected to WMI scope: {scope}");
                _updater = updater;

                return;
            }

            _logger.Error("Could not connect to any WMI scope.");
        }

        public override void ModuleDeactivated(bool isOverride)
        {
            _logger.Information("Deactivate OpenHardwareMonitor Plugin");

            if (isOverride)
                return;

            if (_updater != null)
            {
                _updater.Dispose();
                _updater = null;
            }

            _cache.Clear();
            DataModel.ClearDynamicChildren();
        }

        private void PopulateDynamicDataModels(IEnumerable<Hardware> hardwares, IEnumerable<Sensor> sensors)
        {
            DataModel.ClearDynamicChildren();
            
            int hardwareIdCounter = 0;
            foreach (Hardware hw in hardwares.OrderBy(hw => hw.HardwareType))
            {
                //loop through the hardware,
                //and find all the sensors that hardware has
                IEnumerable<Sensor> children = sensors.Where(s => s.Parent == hw.Identifier);

                //if we don't find any sensors, skip and do the next hardware
                if (!children.Any())
                {
                    continue;
                }
                
                HardwareDynamicDataModel hwDataModel = DataModel.AddDynamicChild(
                    $"{hw.HardwareType}{hardwareIdCounter++}",
                    new HardwareDynamicDataModel(),
                    hw.Name,
                    hw.HardwareType.ToString()
                ).Value;

                //group sensors by type for easier UI navigation.
                //this is also the way the UI of the HardwareMonitor
                //programs displays the sensors, so let's keep that consistent
                foreach (IGrouping<SensorType, Sensor> sensorsOfType in children.GroupBy(s => s.SensorType))
                {
                    SensorTypeDynamicDataModel sensorTypeDataModel = hwDataModel.AddDynamicChild(
                        sensorsOfType.Key.ToString(),
                        new SensorTypeDynamicDataModel()
                    ).Value;

                    int sensorIdCounter = 0;
                    //for each type of sensor, we add all the sensors we found
                    foreach (Sensor sensorOfType in sensorsOfType.OrderBy(s => s.Name))
                    {
                        if (_cache.ContainsKey(sensorOfType.Identifier))
                            continue;
                        
                        //this switch is only useful for the unit of each sensor
                        SensorDynamicDataModel dataModel = sensorsOfType.Key switch
                        {
                            SensorType.Temperature => new TemperatureDynamicDataModel(sensorOfType.Identifier),
                            SensorType.Load => new PercentageDynamicDataModel(sensorOfType.Identifier),
                            SensorType.Level => new PercentageDynamicDataModel(sensorOfType.Identifier),
                            SensorType.Voltage => new VoltageDynamicDataModel(sensorOfType.Identifier),
                            SensorType.SmallData => new SmallDataDynamicDataModel(sensorOfType.Identifier),
                            SensorType.Data => new BigDataDynamicDataModel(sensorOfType.Identifier),
                            SensorType.Power => new PowerDynamicDataModel(sensorOfType.Identifier),
                            SensorType.Fan => new FanDynamicDataModel(sensorOfType.Identifier),
                            SensorType.Throughput => new ThroughputDynamicDataModel(sensorOfType.Identifier),
                            SensorType.Clock => new ClockDynamicDataModel(sensorOfType.Identifier),
                            SensorType.Current => new CurrentDynamicDataModel(sensorOfType.Identifier),
                            SensorType.Frequency => new FrequencyDynamicDataModel(sensorOfType.Identifier),
                            SensorType.Flow => new FlowDynamicDataModel(sensorOfType.Identifier),
                            SensorType.Control => new PercentageDynamicDataModel(sensorOfType.Identifier),
                            SensorType.Factor => new SensorDynamicDataModel(sensorOfType.Identifier),
                            SensorType.TimeSpan => new TimeSpanDynamicDataModel(sensorOfType.Identifier),
                            SensorType.Energy => new EnergyDynamicDataModel(sensorOfType.Identifier),
                            SensorType.Noise => new NoiseDynamicDataModel(sensorOfType.Identifier),
                            _ => new SensorDynamicDataModel(sensorOfType.Identifier),
                        };

                        DynamicChild<SensorDynamicDataModel> datamodel = sensorTypeDataModel.AddDynamicChild(
                            (sensorIdCounter++).ToString(),
                            dataModel,
                            sensorOfType.Name
                        );

                        _cache.Add(sensorOfType.Identifier, datamodel);
                    }
                }
            }
        }

        private void UpdateSensorsAndDataModel(double deltaTime)
        {
            foreach (Sensor sensor in _updater.FetchSensors())
            {
                if (_cache.TryGetValue(sensor.Identifier, out DynamicChild<SensorDynamicDataModel> dynamicChild))
                {
                    dynamicChild.Value.CurrentValue = sensor?.Value ?? -1;
                    dynamicChild.Value.Minimum = sensor?.Min ?? -1;
                    dynamicChild.Value.Maximum = sensor?.Max ?? -1;
                }
            }
        }
    }
}
