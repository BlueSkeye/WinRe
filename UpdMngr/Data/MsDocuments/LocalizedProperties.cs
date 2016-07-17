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
    public partial class LocalizedProperties
    {
        [XmlElement(DataType = "language")]
        public string Language { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
    }
}