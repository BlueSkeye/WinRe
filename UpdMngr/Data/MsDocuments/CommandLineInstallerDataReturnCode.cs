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
    public partial class CommandLineInstallerDataReturnCode
    {
        public CommandLineInstallerDataReturnCode()
        {
            this.Reboot = false;
        }

        [XmlElement("LocalizedDescription")]
        public CommandLineInstallerDataReturnCodeLocalizedDescription[] LocalizedDescription { get; set; }
        [XmlAttribute()]
        public int Code { get; set; }
        [XmlAttribute()]
        public InstallationResult Result { get; set; }
        [XmlAttribute()]
        [DefaultValue(false)]
        public bool Reboot { get; set; }
    }
}
