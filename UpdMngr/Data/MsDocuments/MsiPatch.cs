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
    [XmlRoot(Namespace = "http://www.microsoft.com/msi/patch_applicability.xsd", IsNullable = false)]
    public partial class MsiPatch
    {
        [XmlElement("TargetProduct")]
        public MsiPatchTargetProduct[] TargetProduct { get; set; }
        [XmlElement("TargetProductCode")]
        public string[] TargetProductCode { get; set; }
        [XmlElement("ObsoletedPatch")]
        public string[] ObsoletedPatch { get; set; }
        [XmlElement("SequenceData")]
        public MsiPatchSequenceData[] SequenceData { get; set; }
        [XmlAttribute()]
        public string SchemaVersion { get; set; }
        [XmlAttribute()]
        public string PatchGUID { get; set; }
        [XmlAttribute()]
        public int MinMsiVersion { get; set; }
        [XmlIgnore()]
        public bool MinMsiVersionSpecified { get; set; }
        [XmlAttribute()]
        public bool TargetsRTM { get; set; }
        [XmlIgnore()]
        public bool TargetsRTMSpecified { get; set; }
    }
}
