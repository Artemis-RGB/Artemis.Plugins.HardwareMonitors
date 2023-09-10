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
    private HwInfoHardware[] _hardwares;
    private HwInfoSensor[] _sensors;

    private SensorDataModel[] _sensorDataModels;

    private TimedUpdateRegistration _timedUpdate;
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
        
        try
        {
            _reader = new HwInfo64Reader();
            var hwInfoData = _reader.ReadRoot();

            if (hwInfoData.Version != 2)
            {
                _logger.Error("HWiNFO64 protocol version is not 2, please update HWiNFO64 to the latest version");
                return;
            }

            _hardwares = new HwInfoHardware[hwInfoData.HardwareCount];
            _sensors = new HwInfoSensor[hwInfoData.SensorCount];
            _sensorDataModels = new SensorDataModel[hwInfoData.SensorCount];

            PopulateDynamicDataModels();

            _timedUpdate?.Dispose();

            _timedUpdate = AddTimedUpdate(TimeSpan.FromMilliseconds(hwInfoData.PollingPeriod), UpdateSensorsAndDataModel, nameof(UpdateSensorsAndDataModel));

            _logger.Verbose("Started HWiNFO64 memory reader successfully");
        }
        catch (FileNotFoundException e1)
        {
            _logger.Error(e1, "Failed to start HWiNFO64 memory reader. Make sure the \"Shared Memory Support\" option is enabled in HWiNFO64 settings and restart this plugin.");
        }
        catch (Exception e3)
        {
            _logger.Error(e3, "Exception reading HWInfo structure, please show developers.");
        }
    }

    public override void ModuleDeactivated(bool isOverride)
    {
        _reader?.Dispose();
        _sensors = null;
        _hardwares = null;
        _sensorDataModels = null;
    }

    public override void Enable()
    {
    }

    public override void Disable()
    {
    }

    public override void Update(double deltaTime)
    {
        //updates are done only as often as HWiNFO64 polls to save resources.
    }

    private void UpdateSensorsAndDataModel(double deltaTime)
    {
        _reader.ReadSensors(_sensors);

        for (var index = 0; index < _sensors.Length; index++)
        {
            var dataModel = _sensorDataModels[index];
            var sensor = _sensors[index];

            dataModel.CurrentValue = sensor.Value;
            dataModel.Average = sensor.ValueAvg;
            dataModel.Minimum = sensor.ValueMin;
            dataModel.Maximum = sensor.ValueMax;
        }
    }

    private void PopulateDynamicDataModels()
    {
        _reader.ReadHardwares(_hardwares);
        _reader.ReadSensors(_sensors);

        var sensorsWithIndex = _sensors
            .Select((s, idx) => (Sensor: s, Index: idx))
            .ToArray();

        for (var i = 0; i < _hardwares.Length; i++)
        {
            var hw = _hardwares[i];

            var childrenWithIndex = sensorsWithIndex
                .Where(s => s.Sensor.ParentIndex == i)
                .ToArray();

            if (!childrenWithIndex.Any())
                continue;

            var hardwareDataModel = DataModel.AddDynamicChild(
                hw.GetId(),
                new HardwareDataModel(),
                hw.NameCustomUtf8,
                hw.NameOriginal
            ).Value;

            foreach (var sensorsOfType in childrenWithIndex.GroupBy(s => s.Sensor.SensorType))
            {
                var sensorTypeDataModel = hardwareDataModel.AddDynamicChild(
                    sensorsOfType.Key.ToString(),
                    new SensorTypeDataModel()
                ).Value;

                //this will make it so the ids are something like:
                //load1, load2, temperature1, temperature2
                //which should be somewhat portable i guess
                var sensorOfTypeIndex = 0;
                foreach (var (sensor, index) in sensorsOfType.OrderBy(s => s.Sensor.LabelOriginal.ToString()))
                {
                    var dataModel = sensorTypeDataModel.AddDynamicChild(
                        $"{sensorsOfType.Key.ToString().ToLower()}{sensorOfTypeIndex++}",
                        new SensorDataModel(),
                        sensor.LabelCustomUtf8,
                        sensor.LabelOriginal
                    );

                    _sensorDataModels[index] = dataModel.Value;
                }
            }
        }
    }
}