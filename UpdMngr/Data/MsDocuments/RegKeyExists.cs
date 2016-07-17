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
    public partial class RegKeyExists
    {
        public RegKeyExists()
        {
            this.RegType32 = false;
        }
        [XmlAttribute()]
        public RegistryKey Key { get; set; }
        [XmlAttribute()]
        public string Subkey { get; set; }
        [XmlAttribute()]
        [DefaultValue(false)]
        public bool RegType32 { get; set; }
    }
}
