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
    public partial class MsiPatchTargetProduct
    {
        public ValidateGUID TargetProductCode { get; set; }
        public string UpdatedProductCode { get; set; }
        public ValidateVersion TargetVersion { get; set; }
        public string UpdatedVersion { get; set; }
        public ValidateLanguage TargetLanguage { get; set; }
        public string UpdatedLanguages { get; set; }
        public ValidateGUID UpgradeCode { get; set; }
        [XmlAttribute()]
        public int MinMsiVersion { get; set; }
        [XmlIgnore()]
        public bool MinMsiVersionSpecified { get; set; }
    }
}
