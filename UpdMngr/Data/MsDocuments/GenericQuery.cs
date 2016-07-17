using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Xml.Serialization;

namespace UpdMngr.Data.MsDocuments
{
    [Serializable()]
    [DebuggerStepThroughAttribute()]
    [DesignerCategoryAttribute("code")]
    [XmlType(AnonymousType = true, Namespace = "http://schemas.microsoft.com/wsus/2002/12/BaseApplicabilityRules.xsd")]
    [XmlRoot(Namespace = "http://schemas.microsoft.com/wsus/2002/12/BaseApplicabilityRules.xsd", IsNullable = false)]
    public partial class GenericQuery
    {
        [XmlAttribute()]
        public string Namespace { get; set; }
        [XmlAttribute()]
        public string Type { get; set; }
        [XmlAttribute()]
        public string Parameter { get; set; }
        [XmlAttribute()]
        public ScalarComparison Comparison { get; set; }
        [XmlIgnore()]
        public bool ComparisonSpecified { get; set; }
        [XmlAttribute()]
        public string Value { get; set; }
    }
}
