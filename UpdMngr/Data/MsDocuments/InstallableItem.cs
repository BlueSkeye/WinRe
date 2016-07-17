using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Xml.Serialization;

namespace UpdMngr.Data.MsDocuments
{
    [Serializable()]
    [DebuggerStepThroughAttribute()]
    [DesignerCategoryAttribute("code")]
    [XmlType(Namespace = "http://schemas.microsoft.com/wsus/2002/12/SoftwareDistributionPackage.xsd")]
    public partial class InstallableItem
    {
        public ApplicabilityRules ApplicabilityRules { get; set; }
        public InstallProperties InstallProperties { get; set; }
        public InstallProperties UninstallProperties { get; set; }
        public object InstallHandlerSpecificData { get; set; }
        public File OriginFile { get; set; }
        [XmlElement("Language", DataType = "language")]
        public string[] Language { get; set; }
        [XmlAttribute()]
        public string ID { get; set; }
    }
}