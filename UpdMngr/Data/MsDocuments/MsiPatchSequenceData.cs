using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Xml.Serialization;

namespace UpdMngr.Data.MsDocuments
{
    [Serializable()]
    [DebuggerStepThroughAttribute()]
    [DesignerCategoryAttribute("code")]
    [XmlType(AnonymousType = true, Namespace = "http://www.microsoft.com/msi/patch_applicability.xsd")]
    public partial class MsiPatchSequenceData
    {
        public string PatchFamily { get; set; }
        public string ProductCode { get; set; }
        public string Sequence { get; set; }
        public int Attributes { get; set; }
        [XmlIgnore()]
        public bool AttributesSpecified { get; set; }
    }
}
