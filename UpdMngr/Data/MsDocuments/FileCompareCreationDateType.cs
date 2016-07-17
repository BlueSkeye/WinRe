using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Xml.Serialization;

namespace UpdMngr.Data.MsDocuments
{
    [Serializable()]
    [DebuggerStepThroughAttribute()]
    [DesignerCategoryAttribute("code")]
    [XmlType(Namespace = "http://schemas.microsoft.com/wsus/2002/12/BaseApplicabilityRules.xsd")]
    public partial class FileCompareCreationDateType
    {
        [XmlAttribute()]
        public string Path { get; set; }
        [XmlAttribute()]
        public ScalarComparison Comparison { get; set; }
        [XmlAttribute()]
        public System.DateTime Created { get; set; }
    }
}
