using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Xml.Serialization;

namespace UpdMngr.Data.MsDocuments
{
    [Serializable()]
    [DebuggerStepThroughAttribute()]
    [DesignerCategoryAttribute("code")]
    [XmlType(AnonymousType = true, Namespace = "http://schemas.microsoft.com/wsus/2002/12/Installers/CommandLineInstallation.xsd")]
    [XmlRoot(Namespace = "http://schemas.microsoft.com/wsus/2002/12/Installers/CommandLineInstallation.xsd", IsNullable = false)]
    public partial class CommandLineInstallerData
    {
        public CommandLineInstallerData()
        {
            this.DefaultResult = InstallationResult.Failed;
            this.RebootByDefault = false;
        }

        [XmlElement("ReturnCode")]
        public CommandLineInstallerDataReturnCode[] ReturnCode { get; set; }
        [XmlElement("WindowsInstallerRepairPath")]
        public string[] WindowsInstallerRepairPath { get; set; }
        [XmlAttribute()]
        public string Program { get; set; }
        [XmlAttribute()]
        public string Arguments { get; set; }
        [XmlAttribute()]
        [DefaultValue(InstallationResult.Failed)]
        public InstallationResult DefaultResult { get; set; }
        [XmlAttribute()]
        [DefaultValue(false)]
        public bool RebootByDefault { get; set; }
    }
}
