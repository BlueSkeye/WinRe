using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Xml.Serialization;

namespace UpdMngr.Data.MsDocuments
{
    [SerializableAttribute()]
    [DebuggerStepThroughAttribute()]
    [DesignerCategoryAttribute("code")]
    [XmlTypeAttribute(AnonymousType = true, Namespace = "http://schemas.microsoft.com/wsus/2002/12/BaseApplicabilityRules.xsd")]
    [XmlRootAttribute(Namespace = "http://schemas.microsoft.com/wsus/2002/12/BaseApplicabilityRules.xsd", IsNullable = false)]
    public partial class WindowsVersion
    {
        public WindowsVersion()
        {
            this.AllSuitesMustBePresent = false;
        }

        [XmlAttribute()]
        public ScalarComparison Comparison { get; set; }
        [XmlIgnore()]
        public bool ComparisonSpecified { get; set; }
        [XmlAttribute()]
        public uint MajorVersion { get; set; }
        [XmlIgnore()]
        public bool MajorVersionSpecified { get; set; }
        [XmlAttribute()]
        public uint MinorVersion { get; set; }
        [XmlIgnore()]
        public bool MinorVersionSpecified { get; set; }
        [XmlAttribute()]
        public uint BuildNumber { get; set; }
        [XmlIgnore()]
        public bool BuildNumberSpecified { get; set; }
        [XmlAttribute()]
        public ushort ServicePackMajor { get; set; }
        [XmlIgnore()]
        public bool ServicePackMajorSpecified { get; set; }
        [XmlAttribute()]
        public ushort ServicePackMinor { get; set; }
        [XmlIgnore()]
        public bool ServicePackMinorSpecified { get; set; }
        [XmlAttribute()]
        [DefaultValue(false)]
        public bool AllSuitesMustBePresent { get; set; }
        [XmlAttribute()]
        public ushort SuiteMask { get; set; }
        [XmlIgnore()]
        public bool SuiteMaskSpecified { get; set; }
        [XmlAttribute()]
        public ushort ProductType { get; set; }
        [XmlIgnore()]
        public bool ProductTypeSpecified { get; set; }
    }
}