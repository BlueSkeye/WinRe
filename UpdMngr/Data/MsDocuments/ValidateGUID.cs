using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Xml.Serialization;

namespace UpdMngr.Data.MsDocuments
{
    [Serializable()]
    [DebuggerStepThroughAttribute()]
    [DesignerCategoryAttribute("code")]
    [XmlType(Namespace = "http://www.microsoft.com/msi/patch_applicability.xsd")]
    public partial class ValidateGUID
    {
        [XmlAttribute()]
        public bool Validate { get; set; }
        [XmlIgnore()]
        public bool ValidateSpecified { get; set; }
        [XmlText()]
        public string Value { get; set; }
    }
}
