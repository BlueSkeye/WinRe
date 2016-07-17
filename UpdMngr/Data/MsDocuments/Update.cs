using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Xml.Serialization;

namespace UpdMngr.Data.MsDocuments
{
    [Serializable()]
    [DebuggerStepThroughAttribute()]
    [DesignerCategoryAttribute("code")]
    [XmlType(AnonymousType = true, Namespace = "http://schemas.microsoft.com/msus/2002/12/Update")]
    [XmlRoot(Namespace = "http://schemas.microsoft.com/msus/2002/12/Update", IsNullable = false)]
    public partial class Update
    {
        public UpdateIdentity UpdateIdentity { get; set; }
        public Properties1 Properties { get; set; }
        [XmlArrayItem("LocalizedProperties", typeof(object), IsNullable = false)]
        public object[][] LocalizedPropertiesCollection { get; set; }
        [XmlElement("ApplicationSpecificData", typeof(object), Namespace = "http://schemas.microsoft.com/wsus/2002/12/SoftwareDistributionPackage.xsd")]
        [XmlElement("UpdateSpecificData", typeof(SoftwareDistributionPackagePackageTypeSpecificDataUpdateSpecificData), Namespace = "http://schemas.microsoft.com/wsus/2002/12/SoftwareDistributionPackage.xsd")]
        public object Item { get; set; }
        public UpdateIsInstallable IsInstallable { get; set; }
        [XmlArrayItem("PackageID", IsNullable = false)]
        public string[] SupersededPackages { get; set; }
        [XmlArrayItem("Prerequisites", IsNullable = false)]
        [XmlArrayItem("AtLeastOne", IsNullable = false, NestingLevel = 1)]
        public UpdatePrerequisitesAtLeastOne[][] Relationships { get; set; }
        public UpdateApplicabilityRules ApplicabilityRules { get; set; }
        public Category HandlerSpecificData { get; set; }
    }
}
