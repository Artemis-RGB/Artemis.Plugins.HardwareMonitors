using System.Xml;

namespace Artemis.Plugins.HardwareMonitors.Aida64
{
    internal class AidaElement
    {
        public string Type { get; set; }
        public string Id { get; set; }
        public string Label { get; set; }
        public string Value { get; set; }

        public static AidaElement FromXmlElement(XmlElement item)
        {
            return new AidaElement
            {
                Type = item.Name,
                Id = item["id"].InnerText,
                Label = item["label"].InnerText,
                Value = item["value"].InnerText
            };
        }

        public override string ToString()
        {
            return $"{Type} - {Label} - {Value}";
        }
    }
}