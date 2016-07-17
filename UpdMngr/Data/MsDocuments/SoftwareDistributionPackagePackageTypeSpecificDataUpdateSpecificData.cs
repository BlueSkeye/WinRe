using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Xml.Serialization;

namespace UpdMngr.Data.MsDocuments
{
    [Serializable()]
    [DebuggerStepThroughAttribute()]
    [DesignerCategoryAttribute("code")]
    [XmlType(AnonymousType = true, Namespace = "http://schemas.microsoft.com/wsus/2002/12/SoftwareDistributionPackage.xsd")]
    [XmlRoot("UpdateSpecificData", Namespace = "http://schemas.microsoft.com/wsus/2002/12/SoftwareDistributionPackage.xsd", IsNullable = false)]
    public partial class SoftwareDistributionPackagePackageTypeSpecificDataUpdateSpecificData
    {
        public string SecurityBulletinID { get; set; }
        [XmlElement("CveID")]
        public string[] CveID { get; set; }
        public string KBArticleID { get; set; }
        [XmlAttribute()]
        public MsrcSeverity MsrcSeverity { get; set; }
        [XmlIgnore()]
        public bool MsrcSeveritySpecified { get; set; }
        [XmlAttribute()]
        public UpdateClassification UpdateClassification { get; set; }
    }
}