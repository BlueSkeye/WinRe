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
    public partial class CommandLineInstallerDataReturnCodeLocalizedDescription
    {
        [XmlElement("Language", DataType = "language")]
        public string[] Language { get; set; }
        public string Description { get; set; }
    }
}
