using System;
using Artemis.Core.Modules;
using Serilog;

namespace Artemis.Plugins.HardwareMonitors.OpenHardwareMonitor
{
    public class HardwareMonitorActivationRequirement : IModuleActivationRequirement
    {
        public string Scope { get; private set; }

        private bool _cached = false;
        private ProcessActivationRequirement _inner;

        public HardwareMonitorActivationRequirement(string scope, string? processName, string? location = null) {
            Scope = scope;

            _inner = new ProcessActivationRequirement(processName, location);
        }

        public bool Evaluate()
        {
            if (!_inner.Evaluate())
            {
                _cached = false;
                return false;
            }

            if (_cached)
                return true;

            try
            {
                using (var updater = new WmiUpdater(Scope)) 
                {
                    var sensors = updater.FetchSensors();
                    var hardwares = updater.FetchHardwares();

                    _cached = hardwares.Count != 0 && sensors.Count != 0;

                    return _cached;
                }
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public string GetUserFriendlyDescription()
        {
            return _inner.GetUserFriendlyDescription() + $" and the WMI namespace \"{Scope}\" has some data";
        }
    }
}
