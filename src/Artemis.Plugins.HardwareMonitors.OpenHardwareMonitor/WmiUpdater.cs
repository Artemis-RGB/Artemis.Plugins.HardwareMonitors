using System;
using System.Collections.Generic;
using System.Management;

namespace Artemis.Plugins.HardwareMonitors.OpenHardwareMonitor
{
    public class WmiUpdater : IDisposable
    {
        private static readonly ObjectQuery SensorQuery = new ObjectQuery("SELECT * FROM Sensor");
        private static readonly ObjectQuery HardwareQuery = new ObjectQuery("SELECT * FROM Hardware");

        private ManagementScope _scope;
        private ManagementObjectSearcher _sensorSearcher;
        private ManagementObjectSearcher _hardwareSearcher;

        public WmiUpdater(string scope)
        {
            _scope = new ManagementScope(scope, null);
            _scope.Connect();

            _sensorSearcher = new ManagementObjectSearcher(_scope, SensorQuery);
            _hardwareSearcher = new ManagementObjectSearcher(_scope, HardwareQuery);
        }

        public void Dispose()
        {
            _sensorSearcher.Dispose();
            _sensorSearcher = null;

            _hardwareSearcher.Dispose();
            _hardwareSearcher = null;

            _scope = null;
        }

        public List<Sensor> FetchSensors()
        {
            return Sensor.FromCollection(_sensorSearcher.Get());
        }

        public List<Hardware> FetchHardwares()
        {
            return Hardware.FromCollection(_hardwareSearcher.Get());
        }
    }
}
