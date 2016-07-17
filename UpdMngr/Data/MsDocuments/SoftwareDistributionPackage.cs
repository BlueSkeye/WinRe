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
    [XmlRoot(Namespace = "http://schemas.microsoft.com/wsus/2002/12/SoftwareDistributionPackage.xsd", IsNullable = false)]
    public partial class SoftwareDistributionPackage
    {
        public Properties Properties { get; set; }
        [XmlElement("LocalizedProperties")]
        public LocalizedProperties[] LocalizedProperties { get; set; }
        [XmlElement("ApplicationSpecificData", typeof(object))]
        [XmlElement("UpdateSpecificData", typeof(SoftwareDistributionPackagePackageTypeSpecificDataUpdateSpecificData))]
        public object Item { get; set; }
        public SoftwareDistributionPackageIsInstallable IsInstallable { get; set; }
        public SoftwareDistributionPackageIsInstalled IsInstalled { get; set; }
        [XmlArrayItem("PackageID", IsNullable = false)]
        public string[] SupersededPackages { get; set; }
        [XmlArrayItem("AtLeastOne", typeof(string[]), IsNullable = false)]
        [XmlArrayItem("PackageID", typeof(string), IsNullable = false, NestingLevel = 1)]
        public string[][][] Prerequisites { get; set; }
        [XmlArrayItem("PackageID", IsNullable = false)]
        public string[] BundledPackages { get; set; }
        [XmlElement("InstallableItem")]
        public InstallableItem[] InstallableItem { get; set; }
        [XmlAttribute()]
        public string SchemaVersion { get; set; }
    }
}