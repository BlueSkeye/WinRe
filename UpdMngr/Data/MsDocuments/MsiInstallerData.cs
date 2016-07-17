using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Xml.Serialization;

namespace UpdMngr.Data.MsDocuments
{
    [Serializable()]
    [DebuggerStepThroughAttribute()]
    [DesignerCategoryAttribute("code")]
    [XmlType(AnonymousType = true, Namespace = "http://schemas.microsoft.com/wsus/2002/12/Installers/MsiInstallation.xsd")]
    [XmlRoot(Namespace = "http://schemas.microsoft.com/wsus/2002/12/Installers/MsiInstallation.xsd", IsNullable = false)]
    public partial class MsiInstallerData
    {
        [XmlAttribute()]
        public string CommandLine { get; set; }
        [XmlAttribute()]
        public string UninstallCommandLine { get; set; }
        [XmlAttribute()]
        public string MsiFile { get; set; }
        [XmlAttribute()]
        public string ProductCode { get; set; }
    }
}
