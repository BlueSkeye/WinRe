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
    [XmlRoot("MsiPatchMetadata", Namespace = "http://schemas.microsoft.com/wsus/2002/12/MsiApplicabilityRules.xsd", IsNullable = false)]
    public partial class MsiPatchMetadataStuff
    {
        [XmlElement(Namespace = "http://www.microsoft.com/msi/patch_applicability.xsd")]
        public MsiPatch MsiPatch { get; set; }
        public string FallbackPatchCode { get; set; }
    }
}
