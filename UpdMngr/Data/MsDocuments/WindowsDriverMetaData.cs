using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Xml.Serialization;

namespace UpdMngr.Data.MsDocuments
{
    [Serializable()]
    [DebuggerStepThroughAttribute()]
    [DesignerCategoryAttribute("code")]
    [XmlType(Namespace = "http://schemas.microsoft.com/wsus/2002/12/Installers/WindowsDriver.xsd")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://schemas.microsoft.com/wsus/2002/12/Installers/WindowsDriver.xsd", IsNullable = false)]
    public partial class WindowsDriverMetaData
    {
        [XmlElement("CompatibleProvider")]
        public string[] CompatibleProvider { get; set; }
        [XmlAttribute()]
        public string HardwareID { get; set; }
        [XmlAttribute(DataType = "date")]
        public System.DateTime DriverVerDate { get; set; }
        [XmlAttribute()]
        public string DriverVerVersion { get; set; }
        [XmlAttribute()]
        public uint WhqlDriverID { get; set; }
        [XmlAttribute()]
        public DriverClass Class { get; set; }
        [XmlAttribute()]
        public string Company { get; set; }
        [XmlAttribute()]
        public string Provider { get; set; }
        [XmlAttribute()]
        public string Manufacturer { get; set; }
        [XmlAttribute()]
        public string Model { get; set; }
    }
}