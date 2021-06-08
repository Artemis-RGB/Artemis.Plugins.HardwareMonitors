using Artemis.Core;
using Artemis.Core.DataModelExpansions;
using Artemis.Core.Modules;
using Artemis.Plugins.DataModelExpansions.Aida64.DataModels;
using Serilog;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.MemoryMappedFiles;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Xml;

namespace Artemis.Plugins.HardwareMonitors.Aida64
{
    public class Aida64Module : Module<Aida64DataModel>
    {
        private readonly ILogger _logger;
        private MemoryMappedFile memoryMappedFile;
        private MemoryMappedViewStream rootStream;
        private List<AidaElement> _aidaElements;

        public Aida64Module(ILogger logger)
        {
            _logger = logger;

            DisplayName = "Aida64";
            DisplayIcon = "Chip";

            ActivationRequirements.Add(new ProcessActivationRequirement("aida64"));
            UpdateDuringActivationOverride = false;
            AddTimedUpdate(TimeSpan.FromSeconds(1), UpdateSensorsAndDataModel, nameof(UpdateSensorsAndDataModel));
        }

        public override void ModuleActivated(bool isOverride)
        {
            if (isOverride)
                return;

            const int maxRetries = 10;
            bool started = false;

            for (int i = 0; i < maxRetries; i++)
            {
                try
                {
                    memoryMappedFile = MemoryMappedFile.OpenExisting("AIDA64_SensorValues", MemoryMappedFileRights.Read);
                    rootStream = memoryMappedFile.CreateViewStream(
                        0,
                        0,
                        MemoryMappedFileAccess.Read);
                    _aidaElements = new List<AidaElement>();
                    started = true;
                    return;
                }
                catch
                {
                    _logger.Error("Failed to start AIDA64 memory reader. Retrying...");
                    Thread.Sleep(500);
                }
            }

            if (!started)
                throw new ArtemisPluginException("Could not find the aida64 memory mapped file");
        }

        public override void ModuleDeactivated(bool isOverride)
        {
            if (isOverride)
                return;

            memoryMappedFile?.Dispose();
            rootStream?.Dispose();
        }

        public override void Enable()
        {
        }

        public override void Update(double deltaTime)
        {
        }

        public override void Disable()
        {
        }

        private void UpdateSensorsAndDataModel(double deltaTime)
        {
            ReadAidaSensors();
            UpdateDataModels();
        }

        private void UpdateDataModels()
        {
            foreach (var item in _aidaElements)
            {
                if (!DataModel.TryGetDynamicChild(item.Id, out var dm))
                {
                    if (float.TryParse(item.Value, out var floatValue))
                    {
                        dm = DataModel.AddDynamicChild(item.Id, floatValue);
                    }
                    else
                    {
                        dm = DataModel.AddDynamicChild(item.Id, item.Value);
                    }
                }

                switch (dm)
                {
                    case DynamicChild<float> floatDataModel:
                        floatDataModel.Value = float.Parse(item.Value);
                        break;
                    case DynamicChild<string> elementDataModel:
                        elementDataModel.Value = item.Value;
                        break;
                }
            }
        }

        private void ReadAidaSensors()
        {
            XmlDocument doc = new();
            doc.LoadXml(ReadMemoryMappedFile());
            foreach (XmlElement item in doc["aida"])
            {
                switch (item.Name)
                {
                    //case "sys":
                    //    break;
                    //case "fan":
                    //    break;
                    //case "temp":
                    //    break;
                    //case "volt":
                    //    break;
                    //case "pwr":
                    //    break;
                    //case "curr":
                    //    break;
                    //case "duty":
                    //    break;

                    default:
                        _aidaElements.Add(AidaElement.FromXmlElement(item));
                        break;
                }
            }
        }

        private string ReadMemoryMappedFile()
        {
            rootStream.Seek(0, SeekOrigin.Begin);

            var sb = new StringBuilder();
            //since there's no root element we have to add one ourselves
            sb.Append("<aida>");

            int c;
            while ((c = rootStream.ReadByte()) > 0)
                sb.Append((char)c);

            sb.Append("</aida>");

            return sb.ToString();
        }
    }
}