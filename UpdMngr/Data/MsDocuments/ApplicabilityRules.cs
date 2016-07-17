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
    public partial class ApplicabilityRules
    {
        public ApplicabilityRulesIsInstalled IsInstalled { get; set; }
        public ApplicabilityRulesIsSuperseded IsSuperseded { get; set; }
        public ApplicabilityRulesIsInstallable IsInstallable { get; set; }
        [XmlArrayItem("ApplicabilityMetadataElement", IsNullable = false)]
        public object[] Metadata { get; set; }
    }
}