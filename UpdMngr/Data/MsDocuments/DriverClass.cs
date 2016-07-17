using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Xml.Serialization;

namespace UpdMngr.Data.MsDocuments
{
    [Serializable()]
    [XmlType(Namespace = "http://schemas.microsoft.com/wsus/2002/12/Installers/WindowsDriver.xsd")]
    public enum DriverClass
    {
        Video,
        Sound,
        Printers,
        Modems,
        Cameras,
        Scanners,
        Monitors,
        Networking,
        [XmlEnum("Input Devices")]
        InputDevices,
        Storage,
        [XmlEnum("Personal Computers")]
        PersonalComputers,
        [XmlEnum("Servers and Cluster Solutions")]
        ServersandClusterSolutions,
        [XmlEnum("Other Hardware")]
        OtherHardware,
    }
}