using Artemis.Core;
using Artemis.Core.Modules;
using Artemis.Plugins.HardwareMonitors.HWiNFO64.DataModels;
using Serilog;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;

namespace Artemis.Plugins.HardwareMonitors.HWiNFO64;

[PluginFeature(AlwaysEnabled = true)]
public class HwInfoModule : Module<HwInfoDataModel>
{
    private readonly ILogger _logger;

    private HwInfo64Reader _reader;
    private TimedUpdateRegistration _timedUpdate;

    private HwInfoRoot _hwInfoRoot;
    private HwInfoSensor[] _sensors;
    private SensorDynamicDataModel[] _dataModels;

    public override List<IModuleActivationRequirement> ActivationRequirements { get; } = new() { new ProcessActivationRequirement("HWiNFO64") };

    public HwInfoModule(ILogger logger)
    {
        _logger = logger;
        UpdateDuringActivationOverride = false;
    }

    public override void ModuleActivated(bool isOverride)
    {
        if (isOverride)
            return;

        const int MAX_RETRIES = 10;
        var started = false;

        for (var i = 0; i < MAX_RETRIES; i++)
        {
            try
            {
                _reader = new HwInfo64Reader();
                _hwInfoRoot = _reader.ReadRoot();
                
                if (_hwInfoRoot.Version != 2)
                    throw new ArtemisPluginException("HWiNFO64 protocol version is not 2, please update HWiNFO64 to the latest version");
                
                _sensors = new HwInfoSensor[_hwInfoRoot.SensorCount];
                _dataModels = new SensorDynamicDataModel[_hwInfoRoot.SensorCount];
                
                PopulateDynamicDataModels();

                _timedUpdate?.Dispose();

                _timedUpdate = AddTimedUpdate(TimeSpan.FromMilliseconds(_hwInfoRoot.PollingPeriod), UpdateSensorsAndDataModel, nameof(UpdateSensorsAndDataModel));

                started = true;
                _logger.Information("Started HWiNFO64 memory reader successfully");
                return;
            }
            catch (FileNotFoundException e1)
            {
                _logger.Error(e1, "Failed to start HWiNFO64 memory reader. Retrying...");
                Thread.Sleep(500);
            }
            catch (ArtemisPluginException e2)
            {
                _logger.Error(e2, "Failed to start HWiNFO64 memory reader. Please update HWiNFO64 to the latest version.");
                break;
            }
            catch (Exception e3)
            {
                _logger.Error(e3, "Exception reading HWInfo structure, please show developers.");
                break;
            }
        }

        if (!started)
            throw new ArtemisPluginException("Could not find the HWiNFO64 memory mapped file");
    }

    public override void ModuleDeactivated(bool isOverride)
    {
        _reader?.Dispose();
        _sensors = null;
        _dataModels = null;
        _hwInfoRoot = default;
        _sensors = null;
    }

    public override void Enable() { }

    public override void Disable() { }

    public override void Update(double deltaTime)
    {
        //updates are done only as often as HWiNFO64 polls to save resources.
    }

    private void UpdateSensorsAndDataModel(double deltaTime)
    {
        _reader.ReadAllSensors(_sensors);

        for (var index = 0; index < _sensors.Length; index++)
        {
            var dataModel = _dataModels[index];
            var sensor = _sensors[index];

            dataModel.CurrentValue = sensor.Value;
            dataModel.Average = sensor.ValueAvg;
            dataModel.Minimum = sensor.ValueMin;
            dataModel.Maximum = sensor.ValueMax;
        }
    }

    private void PopulateDynamicDataModels()
    {
        _reader.ReadAllSensors(_sensors);
        
        for (var i = 0; i < _hwInfoRoot.HardwareCount; i++)
        {
            var hw = _reader.ReadHardware(i);
            
            var childrenWithIndex = _sensors.Select((sensor, index) => (sensor, index)).Where(re => re.sensor.ParentIndex == i).ToArray();

            if (!childrenWithIndex.Any())
                continue;

            var hardwareDataModel = DataModel.AddDynamicChild(
                hw.GetId(),
                new HardwareDynamicDataModel(),
                hw.NameCustomUtf8,
                hw.NameOriginal
            ).Value;

            foreach (var sensorsOfType in childrenWithIndex.GroupBy(s => s.sensor.SensorType))
            {
                var sensorTypeDataModel = hardwareDataModel.AddDynamicChild(
                    sensorsOfType.Key.ToString(),
                    new SensorTypeDynamicDataModel()
                ).Value;

                //this will make it so the ids are something like:
                //load1, load2, temperature1, temperature2
                //which should be somewhat portable i guess
                var sensorOfTypeIndex = 0;
                foreach (var (sensor, index) in sensorsOfType.OrderBy(s => s.sensor.LabelOriginal.ToString()))
                {
                    var dataModel = sensorTypeDataModel.AddDynamicChild(
                        $"{sensorsOfType.Key.ToString().ToLower()}{sensorOfTypeIndex++}",
                        new SensorDynamicDataModel(),
                        sensor.LabelCustomUtf8,
                        sensor.LabelOriginal
                    );

                    _dataModels[index] = dataModel.Value;
                }
            }
        }
    }
}