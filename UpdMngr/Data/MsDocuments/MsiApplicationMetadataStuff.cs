using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Xml.Serialization;

namespace UpdMngr.Data.MsDocuments
{
    [Serializable()]
    [DebuggerStepThroughAttribute()]
    [DesignerCategoryAttribute("code")]
    [XmlType(Namespace = "http://schemas.microsoft.com/wsus/2002/12/MsiApplicabilityRules.xsd")]
    [XmlRoot("MsiApplicationMetadata", Namespace = "http://schemas.microsoft.com/wsus/2002/12/MsiApplicabilityRules.xsd", IsNullable = false)]
    public partial class MsiApplicationMetadataStuff
    {
        public string ProductCode { get; set; }
    }
}