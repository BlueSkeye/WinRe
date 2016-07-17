using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Xml.Serialization;

namespace UpdMngr.Data.MsDocuments
{
    [Serializable()]
    [DebuggerStepThroughAttribute()]
    [DesignerCategoryAttribute("code")]
    [XmlType(TypeName = "Properties", Namespace = "http://schemas.microsoft.com/msus/2002/12/Update")]
    public partial class Properties1
    {
        public Properties1()
        {
            this.UpdateType = UpdateType1.Software;
        }

        [XmlAttribute(DataType = "language")]
        public string DefaultPropertiesLanguage { get; set; }
        [XmlAttribute()]
        [DefaultValue(UpdateType1.Software)]
        public UpdateType1 UpdateType { get; set; }
        [XmlAttribute()]
        public bool ExplicitlyDeployable { get; set; }
        [XmlAttribute()]
        public bool PerUser { get; set; }
        [XmlAttribute()]
        public bool IsPublic { get; set; }
        [XmlAttribute(DataType = "anyURI")]
        public string Handler { get; set; }
        [XmlAttribute()]
        public PublicationState PublicationState { get; set; }
        [XmlAttribute()]
        public System.DateTime CreationDate { get; set; }
        [XmlAttribute()]
        public string PublisherID { get; set; }
    }

    [Serializable()]
    [DebuggerStepThroughAttribute()]
    [DesignerCategoryAttribute("code")]
    [XmlType(Namespace = "http://schemas.microsoft.com/wsus/2002/12/SoftwareDistributionPackage.xsd")]
    public partial class Properties
    {
        public Properties()
        {
            this.CanSourceBeRequired = false;
            this.RestrictToClientServicingApi = false;
            this.UpdateType = UpdateType.Software;
        }

        [XmlElement("MoreInfoUrl", DataType = "anyURI")]
        public string[] MoreInfoUrl { get; set; }
        [XmlElement(DataType = "anyURI")]
        public string SupportUrl { get; set; }
        [XmlElement("ProductName")]
        public string[] ProductName { get; set; }
        [XmlAttribute()]
        public string PackageID { get; set; }
        [XmlAttribute()]
        public System.DateTime CreationDate { get; set; }
        [XmlAttribute()]
        [DefaultValue(false)]
        public bool CanSourceBeRequired { get; set; }
        [XmlAttribute()]
        public string VendorName { get; set; }
        [XmlAttribute()]
        public PublicationState PublicationState { get; set; }
        [XmlIgnore()]
        public bool PublicationStateSpecified { get; set; }
        [XmlAttribute()]
        [DefaultValue(false)]
        public bool RestrictToClientServicingApi { get; set; }
        [XmlAttribute()]
        [DefaultValue(UpdateType.Software)]
        public UpdateType UpdateType { get; set; }
    }
}