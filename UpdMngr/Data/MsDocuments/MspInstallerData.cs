using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Xml.Serialization;

namespace UpdMngr.Data.MsDocuments
{
    [Serializable()]
    [DebuggerStepThroughAttribute()]
    [DesignerCategoryAttribute("code")]
    [XmlType(AnonymousType = true, Namespace = "http://schemas.microsoft.com/wsus/2002/12/Installers/MspInstallation.xsd")]
    [XmlRoot(Namespace = "http://schemas.microsoft.com/wsus/2002/12/Installers/MspInstallation.xsd", IsNullable = false)]
    public partial class MspInstallerData
    {
        public string MspFileName { get; set; }
        [XmlAttribute()]
        public string PatchCode { get; set; }
        [XmlAttribute()]
        public string FullFilePatchCode { get; set; }
        [XmlAttribute()]
        public string CommandLine { get; set; }
        [XmlAttribute()]
        public string UninstallCommandLine { get; set; }
    }
}
